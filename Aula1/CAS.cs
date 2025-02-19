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

