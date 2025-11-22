using BL;

internal class Program
{
    async static Task Main(string[] args)
    {
        var codeGenerator = new CodeGenerator();
        await codeGenerator.GenerateFromOneFile(@"E:\Dash Board\Resources\WORK & Practice\Code-Sprint\English101.pdf", "Generate only 2 question");
    }

}