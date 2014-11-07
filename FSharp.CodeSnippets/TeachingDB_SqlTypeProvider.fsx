#r "System.Data.Linq.dll"
#r "System.Data.Entity.dll"
#r "FSharp.Data.TypeProviders.dll"

open System.Data.Linq
open System.Data.Entity
open Microsoft.FSharp.Data.TypeProviders
open System
open System.Data
open System.Data.SqlClient

///
/// Run TeachingDB.edmx.sql in FSharp.CodeSnippets.Data.EF
/// 
type TeachingDbSchema = SqlDataConnection<"Data Source=.;Initial Catalog=TeachingDB;Integrated Security=SSPI;">
let db = TeachingDbSchema.GetDataContext()

db.DataContext.ExecuteCommand("delete [dbo].[Marks]")
db.DataContext.ExecuteCommand("delete [dbo].[Students]")
db.DataContext.ExecuteCommand("delete [dbo].[Subj_Teach]")
db.DataContext.ExecuteCommand("delete [dbo].[Groups]")
db.DataContext.ExecuteCommand("delete [dbo].[Subjects]")
db.DataContext.ExecuteCommand("delete [dbo].[Teachers]")

let teacher1 = new TeachingDbSchema.ServiceTypes.Teachers(FirstName="Joe", LastName="Blow")
let teacher2 = new TeachingDbSchema.ServiceTypes.Teachers(FirstName="Hell", LastName="Master")
let teacher3 = new TeachingDbSchema.ServiceTypes.Teachers(FirstName="Michael", LastName="Myers")
let teacher4 = new TeachingDbSchema.ServiceTypes.Teachers(FirstName="Robin", LastName="Hood")
db.Teachers.InsertOnSubmit(teacher1)
db.Teachers.InsertOnSubmit(teacher2)
db.Teachers.InsertOnSubmit(teacher3)
db.Teachers.InsertOnSubmit(teacher4)
db.DataContext.SubmitChanges()

// Add Groups
let group1 = new TeachingDbSchema.ServiceTypes.Groups(Name="Group1")
let group2 = new TeachingDbSchema.ServiceTypes.Groups(Name="Group2")
db.Groups.InsertOnSubmit(group1)
db.Groups.InsertOnSubmit(group2)
db.DataContext.SubmitChanges()

// Add Subjects
let maths = new TeachingDbSchema.ServiceTypes.Subjects(Title="Maths")
let physics = new TeachingDbSchema.ServiceTypes.Subjects(Title="Physics")
let english = new TeachingDbSchema.ServiceTypes.Subjects(Title="English")
let economics = new TeachingDbSchema.ServiceTypes.Subjects(Title="Economics")
db.Subjects.InsertOnSubmit(maths)
db.Subjects.InsertOnSubmit(physics)
db.Subjects.InsertOnSubmit(english)
db.Subjects.InsertOnSubmit(economics)
db.DataContext.SubmitChanges()

// Add Students
let student1 = new TeachingDbSchema.ServiceTypes.Students(FirstName="Jason", LastName="Rails", Group_Id=group1.Id)
let student2 = new TeachingDbSchema.ServiceTypes.Students(FirstName="Jessica", LastName="Alba", Group_Id=group1.Id)
let student3 = new TeachingDbSchema.ServiceTypes.Students(FirstName="Alice", LastName="Davies", Group_Id=group2.Id)
let student4 = new TeachingDbSchema.ServiceTypes.Students(FirstName="Steve", LastName="Jobs", Group_Id=group2.Id)
db.Students.InsertOnSubmit(student1)
db.Students.InsertOnSubmit(student2)
db.Students.InsertOnSubmit(student3)
db.Students.InsertOnSubmit(student4)
db.DataContext.SubmitChanges()

// Add Marks
let mark1Student1 = new TeachingDbSchema.ServiceTypes.Marks(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=1)
let mark2Student1 = new TeachingDbSchema.ServiceTypes.Marks(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=2)
let mark1Student2 = new TeachingDbSchema.ServiceTypes.Marks(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=3)
let mark2Student2= new TeachingDbSchema.ServiceTypes.Marks(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=4)
let mark1Student3 = new TeachingDbSchema.ServiceTypes.Marks(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=5)
let mark2Student3 = new TeachingDbSchema.ServiceTypes.Marks(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=6)
let mark1Student4 = new TeachingDbSchema.ServiceTypes.Marks(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=7)
db.Marks.InsertOnSubmit(mark1Student1)
db.Marks.InsertOnSubmit(mark2Student1)
db.Marks.InsertOnSubmit(mark1Student2)
db.Marks.InsertOnSubmit(mark2Student2)
db.Marks.InsertOnSubmit(mark1Student3)
db.Marks.InsertOnSubmit(mark2Student3)
db.Marks.InsertOnSubmit(mark1Student1)
db.DataContext.SubmitChanges()

let subjTeach1 = new TeachingDbSchema.ServiceTypes.Subj_Teach(Group_Id=group1.Id, Subject_Id=maths.Id, Teacher_Id=teacher1.Id)
let subjTeach1_2 = new TeachingDbSchema.ServiceTypes.Subj_Teach(Group_Id=group1.Id, Subject_Id=physics.Id, Teacher_Id=teacher1.Id)
let subjTeach2 = new TeachingDbSchema.ServiceTypes.Subj_Teach(Group_Id=group1.Id, Subject_Id=physics.Id, Teacher_Id=teacher2.Id)
let subjTeach3 = new TeachingDbSchema.ServiceTypes.Subj_Teach(Group_Id=group2.Id, Subject_Id=english.Id, Teacher_Id=teacher3.Id)
let subjTeach4 = new TeachingDbSchema.ServiceTypes.Subj_Teach(Group_Id=group2.Id, Subject_Id=economics.Id, Teacher_Id=teacher4.Id)

db.Subj_Teach.InsertOnSubmit(subjTeach1)
db.Subj_Teach.InsertOnSubmit(subjTeach1_2)
db.Subj_Teach.InsertOnSubmit(subjTeach2)
db.Subj_Teach.InsertOnSubmit(subjTeach3)
db.Subj_Teach.InsertOnSubmit(subjTeach4)
db.DataContext.SubmitChanges()

// Print all teachers
query { for teacher in db.Teachers do
        select teacher }
|> Seq.iter (fun teacher -> printfn "First Name - %s, Last Name - %s" teacher.FirstName teacher.LastName)

// Print all teachers and what subjects they are teaching
printfn "Subjects taught by teacher names"
query { for teacher in db.Teachers do
        join subjectTaught in db.Subj_Teach on 
            (teacher.Id = subjectTaught.Teacher_Id)                            
        join subject in db.Subjects on 
            (subjectTaught.Subject_Id = subject.Id)        
        select (teacher.FirstName, teacher.LastName, subject.Title) }
|> Seq.iter (fun (f, l, t) -> printfn "First Name - %s, Last Name - %s, Subject - %s" f l t)


