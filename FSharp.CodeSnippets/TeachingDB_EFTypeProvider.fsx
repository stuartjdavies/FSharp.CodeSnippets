#r "System.Data.Linq.dll"
#r "System.Data.Entity.dll"
#r "FSharp.Data.TypeProviders.dll"

open System.Data.Linq
open System.Data.Entity
open Microsoft.FSharp.Data.TypeProviders
open System
open System.Data
open System.Data.SqlClient
open System.Data.EntityClient
open System.Data.Metadata.Edm

type edmx = EdmxFile<"TeachingDB.edmx", ResolutionFolder="""..\FSharp.CodeSnippets.Data.EF\""">

// Note: Had to change ProviderManifestToken from 2012 -> 2008, Not clear why... Version problem
let cs = "metadata=res://*/TeachingDB.csdl|res://*/TeachingDB.ssdl|res://*/TeachingDB.msl;provider=System.Data.SqlClient;provider connection string='data source=.;initial catalog=TeachingDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework'";

let context = new edmx.TeachingDBModel.TeachingDBEntities(cs)

context.ExecuteStoreCommand("delete [dbo].[Marks]")
context.ExecuteStoreCommand("delete [dbo].[Students]")
context.ExecuteStoreCommand("delete [dbo].[Subj_Teach]")
context.ExecuteStoreCommand("delete [dbo].[Groups]")
context.ExecuteStoreCommand("delete [dbo].[Subjects]")
context.ExecuteStoreCommand("delete [dbo].[Teachers]")

// Add Teachers
let teacher1 = new edmx.TeachingDBModel.Teacher(FirstName="Joe", LastName="Blow")
let teacher2 = new edmx.TeachingDBModel.Teacher(FirstName="Hell", LastName="Master")
let teacher3 = new edmx.TeachingDBModel.Teacher(FirstName="Michael", LastName="Myers")
let teacher4 = new edmx.TeachingDBModel.Teacher(FirstName="Robin", LastName="Hood")

context.AddToTeachers(teacher1)
context.AddToTeachers(teacher2)
context.AddToTeachers(teacher3)
context.AddToTeachers(teacher4)
context.SaveChanges()

// Add Groups
let group1 = new edmx.TeachingDBModel.Group(Name="Group1")
let group2 = new edmx.TeachingDBModel.Group(Name="Group2")
context.AddToGroups(group1)
context.AddToGroups(group2)
context.SaveChanges()

// Add Subjects
let maths = new edmx.TeachingDBModel.Subject(Title="Maths")
let physics = new edmx.TeachingDBModel.Subject(Title="Physics")
let english = new edmx.TeachingDBModel.Subject(Title="English")
let economics = new edmx.TeachingDBModel.Subject(Title="Economics")
context.AddToSubjects(maths)
context.AddToSubjects(physics)
context.AddToSubjects(english)
context.AddToSubjects(economics)
context.SaveChanges()

// Add Students
let student1 = new edmx.TeachingDBModel.Student(FirstName="Jason", LastName="Rails", Group_Id=group1.Id)
let student2 = new edmx.TeachingDBModel.Student(FirstName="Jessica", LastName="Alba", Group_Id=group1.Id)
let student3 = new edmx.TeachingDBModel.Student(FirstName="Alice", LastName="Davies", Group_Id=group2.Id)
let student4 = new edmx.TeachingDBModel.Student(FirstName="Steve", LastName="Jobs", Group_Id=group2.Id)
context.AddToStudents(student1)
context.AddToStudents(student2)
context.AddToStudents(student3)
context.AddToStudents(student4)
context.SaveChanges()

// Add Marks
let mark1Student1 = new edmx.TeachingDBModel.Mark(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=1)
let mark2Student1 = new edmx.TeachingDBModel.Mark(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=2)
let mark1Student2 = new edmx.TeachingDBModel.Mark(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=3)
let mark2Student2= new edmx.TeachingDBModel.Mark(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=4)
let mark1Student3 = new edmx.TeachingDBModel.Mark(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=5)
let mark2Student3 = new edmx.TeachingDBModel.Mark(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=6)
let mark1Student4 = new edmx.TeachingDBModel.Mark(Student_Id=student1.Id, Subject_Id=maths.Id, Date=DateTime.Now,StudentMarks=7)
context.AddToMarks(mark1Student1)
context.AddToMarks(mark2Student1)
context.AddToMarks(mark1Student2)
context.AddToMarks(mark2Student2)
context.AddToMarks(mark1Student3)
context.AddToMarks(mark2Student3)
context.AddToMarks(mark1Student1)
context.SaveChanges()

let subjTeach1 = new edmx.TeachingDBModel.Subj_Teach(Group_Id=group1.Id, Subject_Id=maths.Id, Teacher_Id=teacher1.Id)
let subjTeach1_2 = new edmx.TeachingDBModel.Subj_Teach(Group_Id=group1.Id, Subject_Id=physics.Id, Teacher_Id=teacher1.Id)
let subjTeach2 = new edmx.TeachingDBModel.Subj_Teach(Group_Id=group1.Id, Subject_Id=physics.Id, Teacher_Id=teacher2.Id)
let subjTeach3 = new edmx.TeachingDBModel.Subj_Teach(Group_Id=group2.Id, Subject_Id=english.Id, Teacher_Id=teacher3.Id)
let subjTeach4 = new edmx.TeachingDBModel.Subj_Teach(Group_Id=group2.Id, Subject_Id=economics.Id, Teacher_Id=teacher4.Id)

context.AddToSubj_Teach(subjTeach1)
context.AddToSubj_Teach(subjTeach1_2)
context.AddToSubj_Teach(subjTeach2)
context.AddToSubj_Teach(subjTeach3)
context.AddToSubj_Teach(subjTeach4)
context.SaveChanges()

// Print all teachers
query { for teacher in context.Teachers do
        select teacher }
|> Seq.iter (fun teacher -> printfn "First Name - %s, Last Name - %s" teacher.FirstName teacher.LastName)

// Print all teachers and what subjects they are teaching
printfn "Subjects taught by teacher names"
query { for teacher in context.Teachers do
        join subjectTaught in context.Subj_Teach on 
            (teacher.Id = subjectTaught.Teacher_Id)                            
        join subject in context.Subjects on 
            (subjectTaught.Subject_Id = subject.Id)        
        select (teacher.FirstName, teacher.LastName, subject.Title) }
|> Seq.iter (fun (f, l, t) -> printfn "First Name - %s, Last Name - %s, Subject - %s" f l t)


// Query, update and delete data by using DbContext
// Build a query that uses deferred execution
// Implement lazy loading and eager loading
// Create a run compiled queries
// Query data by using Entity SQL 

