using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using DTOs.Auth;
using Schema;
using Repositories.UserRepository;
using Services;
using Repositories;

namespace Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AuthController : ControllerBase{
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Authentication> _authenticationRepository;
        private readonly IPasswordService _passwordService;

        public AuthController(
            IConfiguration configuration, 
            IUserRepository userRepository,
            IGenericRepository<Authentication> authenticationRepository,
            IPasswordService passwordService
        ){
            _configuration = configuration;
            _userRepository = userRepository;
            _authenticationRepository = authenticationRepository;
            _passwordService = passwordService;
        }

        [HttpPost("Login", Name = "Login")]
        public IResult Login([FromBody]LoginDTO login){
            //validate
            User? user = _userRepository.Login(login.Username, _passwordService.EncryptPassword(login.Password));
            if(user == null){
                return Results.BadRequest(new LoginResponseDTO {
                    Success = false,
                    ErrorMessage = "Username / Password not found."
                });
            }

            return LoginHandler(user);
        }

        [HttpPost("Register", Name = "Register User")]
        public IResult Register([FromBody]RegisterDTO registerDTO){
            //validate inputs
            User? user = _userRepository.GetByEmailAddress(registerDTO.EmailAddress);
            if(user is not null){
                //return result
                return Results.BadRequest(new LoginResponseDTO {
                    Success = false,
                    ErrorMessage = "Email already taken"
                }); 
            }

            //create
            user = _userRepository.Register(registerDTO.FullName, registerDTO.EmailAddress, _passwordService.EncryptPassword(registerDTO.Password));

            if(user is null){
                //return result
                return Results.BadRequest(new LoginResponseDTO {
                    Success = false,
                    ErrorMessage = "Failed to Create User"
                }); 
            }

            return LoginHandler(user);
        }

        private IResult LoginHandler(User? user){
            //check if token valid
            if(user is null){
                 return Results.BadRequest(new LoginResponseDTO {
                    Success = false,
                    ErrorMessage = "Empty User"
                });
            }

            //set token expired
            DateTime tokenExpires = DateTime.UtcNow.AddHours(1);

            //create token
            string token = CreateJWTToken(user, tokenExpires); 

            //check if token valid
            if(string.IsNullOrEmpty(token)){
                 return Results.BadRequest(new LoginResponseDTO {
                    Success = false,
                    ErrorMessage = "Failed to Generate Token"
                });
            }

            //log into auth table
            _authenticationRepository.Create(new Authentication{
                UserId = user.Id,
                Token = token,
                ExpiresAt = tokenExpires
            });

            //return result
            return Results.Ok(new LoginResponseDTO {
                Success = true,
                Token = token
            });
        }

        private string CreateJWTToken(Schema.User user, DateTime tokenExpires){
            //get secret jwt key
            var jwtSecretObj = _configuration.GetSection("JWT:SecretKey");
            string secretKey = jwtSecretObj.Value ?? "";

            //init claims payload
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role)
            };

            //set jwt config
            var token = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha512)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}