using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AppStore.Models.Domain
{
    public class LoadDatabase
    {
        public static async Task InsertarData(DatabaseContext context, UserManager<AplicationUser> usuarioManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
            }
            if (!usuarioManager.Users.Any())
            {
                var usuario = new AplicationUser
                {
                    Nombre = "Lucas Ciminelli",
                    Email = "lucasaciminelli@gmail.com",
                    UserName = "LucasCimi"
                };

                await usuarioManager.CreateAsync(usuario, "PasswordVaxidrez123$"); //password PasswordLucasCiminelli$
                await usuarioManager.AddToRoleAsync(usuario, "ADMIN");
            }

            if (!context.Categorias!.Any())
            {
                await context.Categorias.AddRangeAsync(
                      new Categoria { Nombre = "Drama" },
                      new Categoria { Nombre = "Comedia" },
                      new Categoria { Nombre = "Acción" },
                      new Categoria { Nombre = "Terror" },
                      new Categoria { Nombre = "Aventura" }
                  );

                await context.SaveChangesAsync();
            }

            if (!context.Libros.Any())
            {
                await context.Libros.AddRangeAsync(
                     new Libro { Titulo = "Quijote", CreateDate = "06/07/2024", Imagen = "quijote.jpg", Autor = "Miguel de Cervantes" },
                     new Libro { Titulo = "La Puerta", CreateDate = "06/07/2007", Imagen = "harry.jpg", Autor = "Jorge Ciminelli" }
                 );

                await context.SaveChangesAsync();
            }

            if (!context.LibroCategorias.Any())
            {
                await context.LibroCategorias.AddRangeAsync(
                    new LibroCategoria { LibroId = 1, CategoriaId = 1 },
                    new LibroCategoria { LibroId = 2, CategoriaId = 1 }
                );

                await context.SaveChangesAsync();
            }

        }

    }
}