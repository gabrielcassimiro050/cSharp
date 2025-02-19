using CAS;

Expressao a = 10;
Expressao b = "b";


Expressao x = 0;
Expressao y = 10;
Expressao soma = x + y;
Console.WriteLine(soma);
soma = soma.Simplificar();
Console.WriteLine(soma);