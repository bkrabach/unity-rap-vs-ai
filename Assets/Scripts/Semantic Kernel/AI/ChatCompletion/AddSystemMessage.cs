using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;
using Unity.VisualScripting;

namespace Scripts.SemanticKernel.AI.ChatCompletion
{
    [UnitCategory("Semantic Kernel/AI/Chat Completion")]
    [UnitTitle("Add System Message")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class AddSystemMessage : Unit
    {
        [DoNotSerialize]
        public ValueInput chat;

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
            chat = ValueInput<OpenAIChatHistory>("chat");
            systemMessage = ValueInput<string>("systemMessage");

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<OpenAIChatHistory>("output", (flow) =>
            {
                var chatValue = flow.GetValue<OpenAIChatHistory>(chat);
                var systemMessageValue = flow.GetValue<string>(systemMessage);

                chatValue.AddSystemMessage(systemMessageValue);

                return chatValue;
            });

            Requirement(chat, enter);
            Requirement(systemMessage, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
