using System;

namespace CAS;

public abstract class Expressao
{
    public abstract override string ToString();
    public abstract Expressao Derivar(Simbolo x);   
    public abstract Expressao Simplificar();

    public static Expressao operator +(Expressao a, Expressao b) => new Adicao(a, b);
    public static Expressao operator -(Expressao a, Expressao b) => new Subtracao(a, b);
    public static Expressao operator *(Expressao a, Expressao b) => new Multiplicacao(a, b);
    public static Expressao operator /(Expressao a, Expressao b) => new Divisao(a, b);
    public static readonly Expressao i = new NumeroComplexo(0, 1);    
    //public static Expressao i(Expressao a, Expressao b) => new NumeroComplexo(int.Parse(a.ToString()), int.Parse(b.ToString()));


    public static implicit operator Expressao(int v) => new Numero(v);
    public static implicit operator Expressao(string s) => new Simbolo(s);

    
}

public class Numero : Expressao
{
    int valor;
    public Numero(int v)
    {
        this.valor = v;
    }

    public override string ToString()
    {
        
        return this.valor.ToString();
    }
    public override Expressao Derivar(Simbolo x)
    {
        return new Numero(0);
    }

    public override Expressao Simplificar()
    {
        return this;
    }
}

public class NumeroComplexo : Expressao{
    public int real, imaginaria;
    public NumeroComplexo(int real, int imaginaria)
    {
        this.real = real;
        this.imaginaria = imaginaria;
        
    }

    

    public override string ToString()
    {   
        if(real==0){
            if(imaginaria==1) return "i";
            else return this.imaginaria.ToString()+"i";
        } 
        return $"({this.real} + {this.imaginaria}i)";
    }
    public override Expressao Derivar(Simbolo x)
    {
        return new NumeroComplexo(0, this.imaginaria);
    }
    public override Expressao Simplificar()
    {
        return this;
    }

    public static implicit operator NumeroComplexo((int real, int imaginaria) par) => new NumeroComplexo(par.real,  par.imaginaria);
}

public class Simbolo : Expressao
{
    string simbolo;
    public Simbolo(string s)
    {
        this.simbolo = s;
    }

    public override string ToString()
    {
        return this.simbolo;
    }
    public override Expressao Derivar(Simbolo x)
    {
        if (this.simbolo == x.simbolo)
            return new Numero(1);
        else
            return new Numero(0);
    }

    public override Expressao Simplificar()
    {
        return this;
    }
}

public class Adicao : Expressao
{
    Expressao a, b;
    public Adicao(Expressao a, Expressao b)
    {
        this.a = a;
        this.b = b;
    }

    public override string ToString()
    {
        return $"({a.ToString()} + {b.ToString()})";
    }

    public override Expressao Derivar(Simbolo x)
    {
        return new Adicao(a.Derivar(x), b.Derivar(x));
    }

    public override Expressao Simplificar()
    {
        if(a.ToString() == "0") return new Numero(int.Parse(b.ToString()));
        if(b.ToString() == "0") return new Numero(int.Parse(a.ToString()));
        return new Adicao(a.Simplificar(), b.Simplificar());
    }
}

public class Subtracao : Expressao
{
    Expressao a, b;
    public Subtracao(Expressao a, Expressao b)
    {
        this.a = a;
        this.b = b;
    }

    public override string ToString()
    {
        return $"({a.ToString()} - {b.ToString()})";
    }

    public override Expressao Derivar(Simbolo x)
    {
        return new Subtracao(a.Derivar(x), b.Derivar(x));
    }

    public override Expressao Simplificar()
    {
        if(a.ToString().Equals("0")) return new Numero(int.Parse(b.ToString()));
        if(b.ToString().Equals("0")) return new Numero(int.Parse(a.ToString()));
        return new Subtracao(a, b);   
    }
}

public class Multiplicacao : Expressao
{
    Expressao a, b;
    public Multiplicacao(Expressao a, Expressao b)
    {
        this.a = a;
        this.b = b;
    }

    public override string ToString()
    {
        return $"({a.ToString()} * {b.ToString()})";
    }

    public override Expressao Derivar(Simbolo x)
    {
        return new Adicao(new Multiplicacao(a.Derivar(x), b), new Multiplicacao(a, b.Derivar(x)));
    }

    public override Expressao Simplificar()
    {
        //System.Console.WriteLine(b.GetType().ToString());
        /*if(b is NumeroComplexo bAux){
            //System.Console.WriteLine((NumeroComplexo)b.real);
            return new Adicao(int.Parse(a.ToString()), bAux.imaginaria);
            //return new Adicao(int.Parse(a.ToString()), (NumeroComplexo)b.real);
        }*/

        if(a.ToString() == "0" || b.ToString() == "0") return new Numero(0);
        if(b.ToString() == "1") return a;
        if(a.ToString() == "1") return b;
        return new Multiplicacao(a, b);
    }
}

public class Divisao : Expressao
{
    Expressao a, b;
    public Divisao(Expressao a, Expressao b)
    {
        this.a = a;
        this.b = b;
    }

    public override string ToString()
    {
        return $"({a.ToString()} / {b.ToString()})";
    }

    public override Expressao Derivar(Simbolo x)
    {
        return new Divisao(
            new Subtracao(
                new Multiplicacao(a.Derivar(x), b), 
                new Multiplicacao(a, b.Derivar(x))), 
            new Multiplicacao(b, b));
    }

    public override Expressao Simplificar(){
        //if(b.ToString().Equals("0")) throw new DivisionByZeroException("Divisão por zero");
        return new Divisao(a, b);
    }
}

