namespace Xyrenth.Models
{
    public class Skip
    {
        public Skip(int studentID, int lessonID, string day, string reason, bool valid)
        {
            this.studentID = studentID;
            this.lessonID = lessonID;
            this.day = day;
            this.reason = reason;
            this.valid = valid;
        }

        public int studentID { get; set; }
        public int lessonID { get; set; }
        public string day { get; set; }
        public string reason { get; set; }
        public bool valid { get; set; }
    }
}
