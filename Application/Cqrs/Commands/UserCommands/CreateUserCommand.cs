using Application.Interfaces;
using AutoMapper;
using BCrypt.Net;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cqrs.Commands.UserCommands
{
    public class CreateUserCommand:IRequest<int>
    {
        public string? FristName {  get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
        {
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _context;

            public CreateUserCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                bool isExistedUser= _context.Users.Any(x=>x.Email==request.Email);
                if(isExistedUser)
                {
                    throw new Exception("User is already existed");
                }
                else
                {
                    User user = _mapper.Map<User>(request);
                    user.Password= BCrypt.Net.BCrypt.HashPassword(user.Password);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return user.Id;
                }
            }
        }
    }
}
