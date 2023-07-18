using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;
using Unity.VisualScripting;

namespace Scripts.SemanticKernel.AI.ChatCompletion
{
    [UnitCategory("Semantic Kernel/AI/Chat Completion")]
    [UnitTitle("Add Assistant Message")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class AddAssistantMessage : Unit
    {
        [DoNotSerialize]
        public ValueInput chat;

        [DoNotSerialize]
        public ValueInput assistantMessage;

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
            assistantMessage = ValueInput<string>("assistantMessage");

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<OpenAIChatHistory>("output", (flow) =>
            {
                var chatValue = flow.GetValue<OpenAIChatHistory>(chat);
                var assistantMessageValue = flow.GetValue<string>(assistantMessage);

                chatValue.AddAssistantMessage(assistantMessageValue);

                return chatValue;
            });

            Requirement(chat, enter);
            Requirement(assistantMessage, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
