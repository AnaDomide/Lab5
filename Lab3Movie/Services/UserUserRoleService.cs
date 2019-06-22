using Lab3Movie.Models;
using Lab3Movie.Validators;
using Lab3Movie.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.Services
{
    public interface IUserUserRoleService
    {
        IQueryable<UserUserRoleGetModel> GetHistoryRoleById(int id);
        ErrorsCollection Create(UserUserRolePostModel userUserRolePostModel);

        string GetUserRoleNameById(int id);
    }

    public class UserUserRoleService : IUserUserRoleService
    {
        private MoviesDbContext context;
        private IUserRoleValidator userRoleValidator;


        public UserUserRoleService(IUserRoleValidator userRoleValidator, MoviesDbContext context)
        {
            this.context = context;
            this.userRoleValidator = userRoleValidator;
        }

        public IQueryable<UserUserRoleGetModel> GetHistoryRoleById(int id)
        {
            IQueryable<UserUserRole> userUserRole = context.UserUserRole
                                    .Include(u => u.UserRole)
                                    .AsNoTracking()
                                    .Where(uur => uur.UserId == id)
                                    .OrderBy(uur => uur.StartTime);

            return userUserRole.Select(uur => UserUserRoleGetModel.FromUserUserRole(uur));
        }

        public string GetUserRoleNameById(int id)
        {
            int userRoleId = context.UserUserRole
               .AsNoTracking()
                .FirstOrDefault(uur => uur.UserId == id && uur.EndTime == null)
                .UserRoleId;

            string numeRol = context.UserRole
                  .AsNoTracking()
                  .FirstOrDefault(ur => ur.Id == userRoleId)
                  .Name;

            return numeRol;
        }


        public ErrorsCollection Create(UserUserRolePostModel userUserRolePostModel)
        {
            var errors = userRoleValidator.Validate(userUserRolePostModel, context);
            if (errors != null)
            {
                return errors;
            }

            User user = context.Users
                .FirstOrDefault(u => u.Id == userUserRolePostModel.UserId);

            if (user != null)
            {
                UserRole userRole = context
                               .UserRole
                               .Include(ur => ur.UserUserRole)
                               .FirstOrDefault(ur => ur.Name == userUserRolePostModel.UserRoleName);

                UserUserRole curentUserUserRole = context.UserUserRole
                                .Include(uur => uur.UserRole)
                                .FirstOrDefault(uur => uur.UserId == user.Id && uur.EndTime == null);

                //discutabil, nu ar trebui sa fie null niciodata, adica la Register nu a fost creat bine un defalut Regular role pentru userul respectiv
                if (curentUserUserRole == null)
                {
                    context.UserUserRole.Add(new UserUserRole
                    {
                        User = user,
                        UserRole = userRole,
                        StartTime = DateTime.Now,
                        EndTime = null
                    });

                    context.SaveChanges();
                    return null;
                }

                //inchiderea perioadel de activare pentru un anumit rol
                if (!curentUserUserRole.UserRole.Name.Contains(userUserRolePostModel.UserRoleName))
                {
                    curentUserUserRole.EndTime = DateTime.Now;

                    context.UserUserRole.Add(new UserUserRole
                    {
                        User = user,
                        UserRole = userRole,
                        StartTime = DateTime.Now,
                        EndTime = null
                    });

                    context.SaveChanges();
                    return null;
                }
                else
                {
                    return null;    //trebuie sa trimit eroare ca modificarea nu poate avea loc, rol nou = rol vechi
                }
            }
            return null;    //eroare Nu exista User cu Id-ul specificat

        }


    }
}