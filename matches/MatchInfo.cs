using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menu
{
    public class MatchInfo
    {
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string Score { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public string Status { get; set; }

        // ✅ Icone en fonction du statut
        public string MatchStatusIcon
        {
            get
            {
                return Status switch
                {
                    "live" => "🟢",
                    "finished" => "🔴",
                    "upcoming" => "⚪",
                    _ => "⚪"
                };
            }
        }

        // ✅ Couleur du texte en fonction du statut
        public Color MatchStatusColor
        {
            get
            {
                return Status switch
                {
                    "live" => Colors.Green,
                    "finished" => Colors.Red,
                    "upcoming" => Colors.Gray,
                    _ => Colors.Gray
                };
            }
        }
    }
}
