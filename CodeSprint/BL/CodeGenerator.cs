using Mscc.GenerativeAI;
using System.Diagnostics;
using Mscc.GenerativeAI;

namespace BL
{
    public class CodeGenerator
    {
        public async Task GenerateFromOneFile(string pdfFilePath = @"E:\Dash Board\Resources\WORK & Practice\Code-Sprint\English101.pdf",
                                        string additionalInformation = "")
        {
            string base64data = "";
            var googleAI = new GoogleAI(apiKey: "AIzaSyBCM3o9aMqzE1bCPSKwi-JmhKHlYNffeLs");


            var model = googleAI.GenerativeModel(model: Model.Gemma3);


            string basePrompt = """
                                    You are an AI assistant that helps build the business logic and API layer for an AI-powered Test Bank Generator system. This system processes educational PDFs and extracts multiple-choice questions.

                                    🧩 Your Task:

                                    Develop a C# backend service that performs the following:

                                    Accepts a PDF file via an HTTP POST endpoint.

                                    Validates the file:

                                    Must be a .pdf
                                    File size must not exceed a defined limit (e.g., 5MB)

                                    Processes the PDF content to:

                                    Detect and extract multiple-choice questions.
                                    Extract the question text and associated answer choices.
                                    Identify the correct answer number if present.

                                    Returns the result as JSON, formatted exactly like this:
                                    Your output must be ONLY JSON !!! Without any additional text or explanation
                                    outside the JSON.

                                    For each question:
                                    - Add "difficulty" property (0–10)
                                    - Add "isCodingQuestion": true if the question title contains the word "code" (case-insensitive), otherwise false.

                                    [
                                      {
                                        "text": "What is the capital of France?",
                                        "choices": {
                                          "choice1": "Berlin",
                                          "choice2": "Madrid",
                                          "choice3": "Paris",
                                          "choice4": "Rome"
                                        },
                                        "answerNumber": 3,
                                        "difficulty": 0,
                                        "isCodingQuestion": false
                                      },
                                      {
                                        "text": "Which planet is known as the Red Planet?",
                                        "choices": {
                                          "choice1": "Earth",
                                          "choice2": "Mars",
                                          "choice3": "Venus",
                                          "choice4": "Jupiter"
                                        },
                                        "answerNumber": 2,
                                        "difficulty": 0,
                                        "isCodingQuestion": false
                                      }
                                    ]
                                """;

            string AddFromUserPrompt = basePrompt;

            // Append additional instructions **only when the user sends something**
            if (!string.IsNullOrWhiteSpace(additionalInformation))
            {
                basePrompt += $"\n\nAdditional User Requirements:\n{additionalInformation}";
            }

            if (File.Exists(pdfFilePath))
            {
                base64data = Convert.ToBase64String(File.ReadAllBytes(pdfFilePath));
            }

            var request = new GenerateContentRequest(basePrompt);
            request.AddPart(new InlineData() { Data = base64data, MimeType = "application/pdf" });


            var stopwatch = Stopwatch.StartNew();
           

            var response = await model.GenerateContent(request);

            stopwatch.Stop();
            Console.WriteLine(response.Text);

            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
    }
}

