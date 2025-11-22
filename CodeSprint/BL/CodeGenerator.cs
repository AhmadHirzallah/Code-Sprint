using Mscc.GenerativeAI;
using System.Diagnostics;
using Mscc.GenerativeAI;

namespace DAL
{
    public class CodeGenerator
    {
        public async Task GenerateCode()
        {
            var googleAI = new GoogleAI(apiKey: "AIzaSyBCM3o9aMqzE1bCPSKwi-JmhKHlYNffeLs");



            var model = googleAI.GenerativeModel(model: Model.Gemma3);



            string prompt = """
                
            """;



            var request = new GenerateContentRequest(prompt);


            var stopwatch = Stopwatch.StartNew();
            await request.AddMedia("https://amromainstorage1.blob.core.windows.net/taskalayze/CV%20-%20Omar%20Waleed.pdf2c9567f3-c349-41be-ac83-924381294f47",
                mimeType: "application/pdf");


            var response = await model.GenerateContent(request);

            stopwatch.Stop();
            Console.WriteLine(response.Text);

            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

    }
}
