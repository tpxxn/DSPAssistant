using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAssistant
{
    public static partial class DSPAssistant
    {
        public static void ConsoleSplash()
        {
            Bootstrap.Debug(" ");
            Bootstrap.Debug(".─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─_─.");
            Bootstrap.Debug(@"│  _____   _____ _____                _     _              _             │");
            Bootstrap.Debug(@"│ |  __ \ / ____|  __ \ /\           (_)   | |            | |            │");
            Bootstrap.Debug(@"│ | |  | | (___ | |__) /  \   ___ ___ _ ___| |_ __ _ _ __ | |_           │");
            Bootstrap.Debug(@"│ | |  | |\___ \|  ___/ /\ \ / __/ __| / __| __/ _` | '_ \| __|          │");
            Bootstrap.Debug(@"│ | |__| |____) | |  / ____ \\__ \__ \ \__ \ || (_| | | | | |_           │");
            Bootstrap.Debug(@"│ |_____/|_____/|_| /_/    \_\___/___/_|___/\__\__,_|_| |_|\__|          │");
            Bootstrap.Debug(@"│                                        Version " + Bootstrap._VERSION + " Initializing   │");
            Bootstrap.Debug(@"└────────────────────────────────────────────────────────────────────────┘");
        }
    }
}
