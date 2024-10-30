![readme](https://github.com/user-attachments/assets/56b4c9d4-749d-41df-97ff-dd023a3090dc)

Rick & Morty exercise:

Still ToDo For the exercise:
- a unit test project with a test -> I've only touched upon unit testing a bit so I can't really progress here

Done:
- create a supporting WebApiReader project. I followed a tutorial [Udemy course by a Krystyna - 9/288]
- create a separate project for storing the DTOs and fetching the data into a collection [Krystyna - 9/291 etc]
- do multithreaded loading of the collection [Krystyna - 17/548 etc]
- create a project for storing Models [and add project references for the project] and convert dto -> model [Krystyna - 9/294]
	[with a bit of .Linq for only using living characters]
- add EF and delete/store the data when the console program runs [Trevoir .Net - 3/11 etc]
- added a simple MVC app with scaffolding. [freewheeled loosely related to a course Trevoir .Net - 9/100]
- added an action/view for selected planet [Frank Liu's course on MVC ]
- implement OutputCache & refresh the cache on certain actions
- injecting a HTTP header from-database that is set when an action runs;
- I used filters to keep MVC mostly for what it's for: UI

What I learnt:
- projects cannot cyclically reference eachother (it needed to click in my head)
- when adding tasks to the threadpool you have to be careful with feeding arguments to your tasks, because the task will only really 'read' the parameter once it changes to Running state, leading to a race condition.
- it's possible to use a custom cast to convert from DTOs to the domain model
- you should change the initial catalog of your connect string *before* updating the database (master) with your migration. :-(
- changing projectnames is a bit of a hassle if you also want the foldernames to change.
- at some point during a course exercise - unrelated to the Rick & Morty thing - I 'got' that if you instantiate an object as its interface, you can only access the methods and properties that are available through that interface. But now I had to debug a piece of code and after 2 hours figured out that I fell into that trap... I couldn't access a table through a Dbcontext because I had instantiated a DbContext instead of a class that inherits from it, which contained the definition of the DbSet I was trying to access... theory versus practice.... :-)
- if you have the DbContext setup in a separate project, adding a MVC app with scaffolding becomes trivial (in .Net 8)
- with the courses that I already did, adding an action/view for selected planet took me 15 minutes. that's reassuring.
- adding multiple startup projects works but I see that the MVC will load its first view before the table in database is purged and reloaded by the console project.
- working better with the filter system in MVC, injecting headers through filters
- using dependency injection in filters
- the standard microsoft documentation is extremely cryptic sometimes

What I am not happy about:
- it looks clumsy with all those projects but maybe that's okay
- some methods need refactoring but since it's a learning project I left the clumsyness in.
- i got most of the functionality up and running but most of the code isn't really safe yet. it needs try/catch etc.
- i'm not keen on the way i've added the dbcontext, but at least i could reuse it for both the console and the mvc app
- i'm thinking it's probably better to have one http helper class and let that:
	- fetch the external data
	- CRUD the internal data (so not let the MVC app interact with the DbContext directly but through a to-be-built Web API)

