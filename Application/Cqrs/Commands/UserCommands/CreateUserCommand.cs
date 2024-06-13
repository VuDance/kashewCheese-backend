using Application.Interfaces;
using AutoMapper;
using BCrypt.Net;
using Domain.ApiException;
using Domain.Authentication;
using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cqrs.Commands.UserCommands
{
    public class CreateUserCommand:IRequest<ApiResponse<UserResponse>>
    {
        public string? FristName {  get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>
        {
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _context;

            public CreateUserCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                ApiResponse<UserResponse> response=new ApiResponse<UserResponse>();
                bool isExistedUser= _context.Users.Any(x=>x.Email==request.Email);
                if(isExistedUser)
                {
                    response.Success = false;
                    response.ErrorMessage = "User is already existed";
                    response.Status = 500;
                    return response;
                }
                else
                {
                    User user = _mapper.Map<User>(request);
                    user.Password= BCrypt.Net.BCrypt.HashPassword(user.Password);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    UserResponse userResponse = new UserResponse();
                    userResponse.Message= "Created Successfully";
                    response.Data=userResponse;
                    response.Status = 200;
                    return response;
                }
            }
        }
    }
}
