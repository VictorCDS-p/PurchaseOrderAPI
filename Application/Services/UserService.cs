using PurchaseOrderAPI.Domain.Entities;
using PurchaseOrderAPI.Infrastructure.Data;

namespace PurchaseOrderAPI.Application.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public (User user, string message) Create(CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Nome do usuário é obrigatório.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new Exception("Email é obrigatório.");

            var exists = _context.Users.Any(u => u.Email == dto.Email);
            if (exists)
                throw new Exception("Email já cadastrado.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return (user, $"Usuário '{user.Name}' criado com sucesso.");
        }

        public (List<User> users, string message) GetAll()
        {
            var users = _context.Users.ToList();
            string msg = users.Any() ? $"Encontrados {users.Count} usuários." : "Nenhum usuário cadastrado.";
            return (users, msg);
        }

        public (User user, string message) GetById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id)
                       ?? throw new Exception($"Usuário com ID {id} não encontrado.");
            return (user, $"Usuário encontrado: {user.Name}");
        }
    }
}