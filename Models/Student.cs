namespace Xyrenth.Models
{
    public class Student
    {
        public Student(int id, string secondName, string firstName, string surname)
        {
            this.id = id;
            this.secondName = secondName;
            this.firstName = firstName;
            this.surname = surname;
        }

        public int id { get; set; }
        public string secondName { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
    }
}
