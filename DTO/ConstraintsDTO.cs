using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class ConstraintsDTO
    {
        public List<double> Coefficients { get; set; }//מקדמי האילוץ
        public double Limit { get; set; }//גבול האילוץ
        public string SlackVariable { get; set; } //(משתנה עודף(לשימוש בגדול שווה
        public string SuperlusVariable { get; set; } //(משתנה עודף(לשימוש בקטן שווה

        //סוג האילוץ -קטן שוונה/גדול שווה/שווה
        public ConstraintType type { get; set; }
    }
    public enum ConstraintType
    {
        LessThanOrEqual,//קטן שווה
        GreaterThanOrEqual,//גדול שווה
        Equal//שווה
    }
}



