using wrangler.models;

namespace wrangler.handlers
{
    public class WidgetCss 
    {
        public string Main { get; set; }
        public string Close { get; set; }
        public string Image { get; set; }
    }

    public class WidgetThemeHandler
    {
        public WidgetCss GetClass(Affect affect, bool isChild)
        {
            if (affect.AssignmentMode && !isChild)
            {
                return new WidgetCss {
                    Main = "widget affect assign",
                    Close = "close"
                };
            }

            if (isChild)
            {
                return new WidgetCss {
                    Main =  "widget affect applied",
                    Close = "close"
                };                
            }

            return new WidgetCss {
                Main =  "widget affect",
                Close = "close"
            };
        }

        public WidgetCss GetClass(Combatant combatant)
        {
            if (!combatant.IsActive)
            {
                return new WidgetCss{
                    Main = "widget combatant inactive",
                    Image = "filter-grey",
                    Close = "close"
                };
            }

            if (combatant.IsActive && combatant.TurnTaken)
            {
                return new WidgetCss{
                    Main = "widget combatant inactive",
                    Image = "filter-grey",
                    Close = "close"
                };
            }

            if (combatant.IsActive && combatant.IsTurn)
            {
                return new WidgetCss{
                    Main = "widget combatant turn",
                    Image = "filter-green",
                    Close = "close"
                };
            }

            return new WidgetCss{
                Main = "widget combatant",
                Image = "filter-grey",
                Close = "close"
            };
        }

        public WidgetCss GetClass()
        {
            return new WidgetCss {
                Main = "widget blue",
                Close = "close"
            };
        }
    }
}
