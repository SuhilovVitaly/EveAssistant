using System;

namespace EveAssistant.UI
{
    public class ComboboxItem
    {
        public string Text { get; set; }
        public IntPtr Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}