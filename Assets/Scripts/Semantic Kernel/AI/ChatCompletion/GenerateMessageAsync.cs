using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Unity.VisualScripting;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/AI/ChatCompletion")]
    [UnitTitle("Add User Message")]
    [UnitSubtitle("Add user message")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class GenerateMessageAsync : WaitUnit
    {
        [DoNotSerialize]
        public ValueInput chatCompletion;

        [DoNotSerialize]
        public ValueInput chat;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        private string runResult;

        protected override void Definition()
        {
            chatCompletion = ValueInput<IChatCompletion>("chatCompletion");
            chat = ValueInput<OpenAIChatHistory>("chat");
            
            output = ValueOutput<string>("output", (flow) =>
            {
                if (runResult == null)
                {
                    throw new Exception("no result available");
                }
                return runResult;
            });

            Requirement(chatCompletion, enter);
            Requirement(chat, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }

        protected override IEnumerator Await(Flow flow)
        {
            var chatCompletionValue = flow.GetValue<IChatCompletion>(chatCompletion);
            var chatValue = flow.GetValue<OpenAIChatHistory>(chat);

            var assistantReply = chatCompletionValue.GenerateMessageAsync(chatValue);

            var task = Task.Run(async () =>
            {
                runResult = await chatCompletionValue.GenerateMessageAsync(chatValue);
            });

            yield return new WaitUntil(() => task.IsCompleted);
            yield return exit;
        }
    }
}
