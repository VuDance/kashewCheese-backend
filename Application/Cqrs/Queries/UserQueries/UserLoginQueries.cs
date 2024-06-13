using Application.Interfaces;
using AutoMapper;
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

namespace Application.Cqrs.Queries.UserQueries
{
    public class UserLoginQueries: IRequest<ApiResponse<AuthenticationResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class UserLoginQueriesHandler : IRequestHandler<UserLoginQueries, ApiResponse<AuthenticationResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IGenerateJSONWebToken _generateJSONWebToken;
            public UserLoginQueriesHandler(IApplicationDbContext context, IGenerateJSONWebToken generateJSONWebToken, IMapper mapper)
            {
                _context = context;
                _generateJSONWebToken = generateJSONWebToken;
                _mapper = mapper;
            }
            public async Task<ApiResponse<AuthenticationResponse>> Handle(UserLoginQueries request, CancellationToken cancellationToken)
            {
                AuthenticationResponse authenticationResponse = new AuthenticationResponse();
                ApiResponse<AuthenticationResponse> apiResponse=new ApiResponse<AuthenticationResponse>();
                User existedUser = _context.Users.Where(u=>u.Email == request.Email).First();
                if (existedUser != null)
                {
                    User user = _mapper.Map<User>(request);
                    if (!BCrypt.Net.BCrypt.Verify(request.Password,existedUser.Password))
                    {
                        throw new ApiException(HttpStatusCode.Forbidden,"UnAuthentication");
                    }
                    else
                    {
                       string token= _generateJSONWebToken.GenerateJSONWebToken(user, "ViewUser");
                        authenticationResponse.token = token;
                        apiResponse.Data = authenticationResponse;
                        apiResponse.Status = 200;
                        return apiResponse;
                    }
                }
                else
                {
                    throw new ApiException(HttpStatusCode.Forbidden, "UnAuthentication");
                }
                
            }

        }
    }
}
