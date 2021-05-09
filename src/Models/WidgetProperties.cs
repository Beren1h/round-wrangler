using System;

namespace wrangler.models
{
    public class WidgetProperties
    {
        public string Text { get; set; }
        public string Theme { get; set; }
        public Action RemoveClick { get; set; }
    }
}
