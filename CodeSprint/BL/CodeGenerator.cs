using Mscc.GenerativeAI;
using System.Diagnostics;

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

                                    ?? Your Task:

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

            string finalPrompt = basePrompt;

            // Append additional instructions **only when the user sends something**
            if (!string.IsNullOrWhiteSpace(additionalInformation))
            {
                finalPrompt += $"\n\nAdditional User Requirements:\n{additionalInformation}";
            }

            if (File.Exists(pdfFilePath))
            {
                base64data = Convert.ToBase64String(File.ReadAllBytes(pdfFilePath));
            }

            var request = new GenerateContentRequest(finalPrompt);
            request.AddPart(new InlineData() { Data = base64data, MimeType = "application/pdf" });

            var stopwatch = Stopwatch.StartNew();

            var response = await model.GenerateContent(request);

            stopwatch.Stop();
            Console.WriteLine(response.Text);
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Process PDF and return the AI response as string (useful for API endpoints)
        /// </summary>
        /// <param name="pdfFilePath">Path to the PDF file</param>
        /// <param name="additionalInformation">Additional instructions for AI processing</param>
        /// <returns>AI response as string</returns>
        public async Task<string> GenerateFromOneFileAsync(string pdfFilePath, string additionalInformation = "")
        {
            try
            {
                string base64data = "";
                var googleAI = new GoogleAI(apiKey: "AIzaSyBCM3o9aMqzE1bCPSKwi-JmhKHlYNffeLs");

                var model = googleAI.GenerativeModel(model: Model.Gemma3);

                string basePrompt = """
                                        You are an AI assistant that helps build the business logic and API layer for an AI-powered Test Bank Generator system. This system processes educational PDFs and extracts multiple-choice questions.

                                        ?? Your Task:

                                        Develop a C# backend service that performs the following:

                                        1. Accepts a PDF file via an HTTP POST endpoint.

                                        2. Validates the file:
                                           - Must be a .pdf
                                           - File size must not exceed a defined limit (e.g., 5MB)

                                        3. Processes the PDF content to:
                                           - Detect and extract multiple-choice questions.
                                           - Extract the question text and associated answer choices.
                                           - Identify the correct answer number if present.

                                        4. Returns the result as JSON, formatted exactly like this:
                                           ?? Your output must be ONLY JSON — without any additional text or explanation outside the JSON.

                                        Each question object must include:
                                        - `"difficulty"`: An integer between 0–10.
                                        - `"isCodingQuestion"`: true if the question text contains the word "code" (case-insensitive), otherwise false.

                                        ?? If the PDF or any page contains only unrelated images (e.g., pictures of animals, objects, scenery, or memes) and does not contain valid multiple-choice questions, return an empty string (`""`) as the only output.

                                        ?? JSON Output Example:
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


                string finalPrompt = basePrompt;

                // Append additional instructions **only when the user sends something**
                if (!string.IsNullOrWhiteSpace(additionalInformation))
                {
                    finalPrompt += $"\n\nAdditional User Requirements:\n{additionalInformation}";
                }

                if (File.Exists(pdfFilePath))
                {
                    base64data = Convert.ToBase64String(File.ReadAllBytes(pdfFilePath));
                }
                else
                {
                    throw new FileNotFoundException($"PDF file not found at path: {pdfFilePath}");
                }

                var request = new GenerateContentRequest(finalPrompt);
                request.AddPart(new InlineData() { Data = base64data, MimeType = "application/pdf" });

                var response = await model.GenerateContent(request);

                return response.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing PDF: {ex.Message}", ex);
            }
        }
    }
}