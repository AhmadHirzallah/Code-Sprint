using BL;

internal class Program
{
    async static Task Main(string[] args)
    {
        var codeGenerator = new CodeGenerator();
        await codeGenerator.GenerateCode();
    }

}