using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;
using Unity.VisualScripting;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/AI")]
    [UnitTitle("Create New Chat")]
    [UnitSubtitle("Create new chat")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class CreateNewChat : Unit
    {
        [DoNotSerialize]
        public ValueInput chatCompletion;

        [DoNotSerialize]
        public ValueInput systemMessage;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput enter;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput exit;

        protected override void Definition()
        {
            chatCompletion = ValueInput<IChatCompletion>("chatCompletion");
            systemMessage = ValueInput<string>("systemMessage");

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<OpenAIChatHistory>("output", (flow) =>
            {
                var chatCompletionValue = flow.GetValue<IChatCompletion>(chatCompletion);
                var systemMessageValue = flow.GetValue<string>(systemMessage);

                return (OpenAIChatHistory)chatCompletionValue.CreateNewChat(systemMessageValue);
            });

            Requirement(chatCompletion, enter);
            Requirement(systemMessage, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
