using Microsoft.EntityFrameworkCore;
using BachelorOppgaveBackend.Model;

namespace BachelorOppgaveBackend.PostgreSQL
{
   public class ApplicationDbContext : DbContext
   {
       // This constructor must exist so you can register it as a service
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
       { }

       // Each DB set maps to a table in the database
       public DbSet<UserRole> UsersRoles { get; set; }
       public DbSet<User> Users { get; set; }
       public DbSet<Category> Categories { get; set; }
       public DbSet<Post> Posts { get; set; }
       public DbSet<Status> Statuses { get; set; }
       public DbSet<Comment> Comments { get; set; }
       public DbSet<Vote> Votes { get; set; }
       public DbSet<Favorite> Favorites { get; set; }
       public DbSet<Notification> Notifications { get; set; }
   }
   
   public class ApplicationDbInitializer
   {
       public void Initialize(ApplicationDbContext db)
       {
           // Delete the database before we initialize it. This is common to do during development.
           db.Database.EnsureDeleted();

           // Recreate the database and tables according to our models
           db.Database.EnsureCreated();

           
           // Add UserRoles test data
           var userRole = new[]
           {
               new UserRole("Admin", "Granted full permission"),
               new UserRole("User", "Regular user. Can only manage their own content")
           };

           db.UsersRoles.AddRange(userRole);
           db.SaveChanges(); // Finally save changes
            
           
           // Add Users test data
           var adminUser = db.UsersRoles.Where(t => t.Type == "Admin").FirstOrDefault();
           var normalUser = db.UsersRoles.Where(t => t.Type == "User").FirstOrDefault();

           var user = new[]
           {
                new User(adminUser, Guid.NewGuid(), "Admin", "admin@admin.admin"),
                new User(normalUser, Guid.NewGuid(), "Trine Trynet", "trine@trynet.no"),
                new User(normalUser, Guid.NewGuid(), "Jonny Bravo", "jonny@bravo.no"),
                new User(normalUser, Guid.NewGuid(), "Hans Henrik", "hans@henrik.no")
           };
            
           db.Users.AddRange(user);
           db.SaveChanges();
           
           
           // Add Categories test data
           var category = new[]
           {
               new Category("Ris", "Noe som er negativt"),
               new Category("Ros", "Noe som er positivt"),
               new Category("Funksjonalitet", "Forslag til ny funksjonalitet")
           };
           
           db.Categories.AddRange(category);
           db.SaveChanges();
                   
            // Get users
           var getUsers = db.Users.ToList();
           
           // Add Status test data
           var s1 = new Status(null, "Venter", "Venter på svar");
           var s2 = new Status(null, "Venter", "Venter på svar");;

           var status = new[]
           {
               s1, s2
           };
           
           db.Statuses.AddRange(status);
           db.SaveChanges();
           

           // Add Post test data
           var getRis = db.Categories.Where(r => r.Type == "Ris").FirstOrDefault();
           var getRos = db.Categories.Where(r => r.Type == "Ros").FirstOrDefault();

           var p1 = new Post(getUsers[0], getRis, s1,"Elendig UX design", "Kunne gjort det mye bedre selv");
           var p2 = new Post(getUsers[1], getRos, s2, "Drit bra animasjoner", "10 av 10 animasjoner på forsiden. Dere har flinke utviklere"); 
           
           var post = new[]
           {
              p1, p2
           };
            
           db.Posts.AddRange(post);
           db.SaveChanges();

           var c1 = new Comment(p1, getUsers[2], null, "Helt enig med deg");
           var c1_1 = new Comment(p1, getUsers[1], c1, "Dere er kjempe bra firma. Ikke hør på de andre!");

           var c2 = new Comment(p1, getUsers[0], null, "Nei! Uenig. De har dårlige animasjoner");
           var c2_2 = new Comment(p1, getUsers[2], c2, "Kjempe dårlig ja!");
           
           var c3 = new Comment(p2, getUsers[0], null, "blabla blabla Parent");
           var c3_3 = new Comment(p2, getUsers[2], c3, "ding dong hong pong child 1");
           var c3_3_3 = new Comment(p2, getUsers[1], c3_3, "ding dong hong pong child 2");



           var comment = new[]
           {
            c1, c1_1, c2, c2_2, c3, c3_3, c3_3_3
           };

           db.Comments.AddRange(comment);
           db.SaveChanges();

           var vote = new[]
           {
               new Vote(getUsers[0].Id, p1.Id, true),
               new Vote(getUsers[1].Id, p1.Id, true),
               new Vote(getUsers[2].Id, p1.Id, false),
               new Vote(getUsers[0].Id, p2.Id, true),
               new Vote(getUsers[1].Id, p2.Id, false)
           };
           
           db.Votes.AddRange(vote);
           db.SaveChanges();


           var favoritt = new[]
           {
               new Favorite(getUsers[1], p1),
               new Favorite(getUsers[1], p2)
           };
           
           db.Favorites.AddRange(favoritt);
           db.SaveChanges();
       }
   }
}


