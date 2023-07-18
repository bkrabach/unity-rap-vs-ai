#nullable enable
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
    [UnitTitle("Generate Message")]
    [UnitSubtitle("Generate AI message")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class GenerateMessageAsync : WaitUnit
    {
        [DoNotSerialize]
        public ValueInput? chatCompletion;

        [DoNotSerialize]
        public ValueInput? chat;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput? output;

        [DoNotSerialize]
        public ControlOutput? error;

        [DoNotSerialize]
        public ValueOutput? errorMessage;

        private string? errorMessageValue;
        private string? runResult;

        protected override void Definition()
        {
            base.Definition();

            chatCompletion = ValueInput<IChatCompletion>("chatCompletion");
            chat = ValueInput<OpenAIChatHistory>("chat");

            output = ValueOutput<string>("output", (flow) => runResult ?? "");
            errorMessage = ValueOutput<string?>("errorMessage", (flow) => errorMessageValue);

            error = ControlOutput("error");

            Requirement(chatCompletion, enter);
            Requirement(chat, enter);
            Succession(enter, exit);
            Succession(enter, error);
            Assignment(enter, output);
        }

        protected override IEnumerator Await(Flow flow)
        {
            var chatCompletionValue = flow.GetValue<IChatCompletion>(chatCompletion);
            var chatValue = flow.GetValue<OpenAIChatHistory>(chat);

            var assistantReply = chatCompletionValue.GenerateMessageAsync(chatValue);

            var task = Task.Run(async () =>
            {
                try
                {
                    runResult = await chatCompletionValue.GenerateMessageAsync(chatValue);
                }
                catch (Exception ex)
                {
                    errorMessageValue = ex.Message;
                }
            });

            yield return new WaitUntil(() => task.IsCompleted);
            yield return exit;
        }
    }
}
