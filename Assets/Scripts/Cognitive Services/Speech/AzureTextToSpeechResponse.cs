using System.Collections.Generic;
using Microsoft.CognitiveServices.Speech;

namespace Scripts.CognitiveServices.Speech
{
    public class AzureTextToSpeechResponse
    {
        public List<Viseme> VisemeList;
        public SpeechSynthesisResult Result;
    }
}
