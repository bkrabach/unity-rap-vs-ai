using System.Collections.Generic;
using Microsoft.CognitiveServices.Speech;

namespace Microsoft.Prototyping.AzureSpeech
{
    public class AzureTextToSpeechResponse
    {
        public List<Viseme> VisemeList;
        public SpeechSynthesisResult Result;
    }
}
