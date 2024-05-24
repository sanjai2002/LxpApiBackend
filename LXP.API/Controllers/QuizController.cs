﻿using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;




using System;

using System.Collections.Generic;

using System.Linq;

namespace LXP.Api.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class QuizController : ControllerBase

    {

        private readonly IQuizService _quizService;

        private readonly IQuizFeedbackService _quizFeedbackService;

        public QuizController(IQuizService quizService, IQuizFeedbackService quizFeedbackService)

        {

            _quizService = quizService;

            _quizFeedbackService = quizFeedbackService;

        }

        [HttpGet("{id}")]

        public ActionResult<QuizDto> GetQuizById(Guid id)

        {

            var quiz = _quizService.GetQuizById(id);

            if (quiz == null)

                throw new Exception($"Quiz with id {id} not found.");

            var quizResponse = new

            {

                quiz.QuizId,

                quiz.NameOfQuiz,

                quiz.Duration,

                quiz.PassMark,

                quiz.AttemptsAllowed // Include AttemptsAllowed in the response

            };

            return Ok(quizResponse);

        }

        [HttpGet]

        public ActionResult<IEnumerable<QuizDto>> GetAllQuizzes()

        {

            var quizzes = _quizService.GetAllQuizzes();

            var quizResponse = quizzes.Select(quiz => new

            {

                quiz.QuizId,

                quiz.NameOfQuiz,

                quiz.Duration,

                quiz.PassMark,

                quiz.AttemptsAllowed // Include AttemptsAllowed in the response

            });

            return Ok(quizResponse);

        }

        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]

        public ActionResult CreateQuiz([FromBody] CreateQuizDto request)

        {

            var quizId = Guid.NewGuid(); // Generate QuizId

            var createdBy = "System"; // Set createdBy

            var createdAt = DateTime.UtcNow; // Set createdAt

            var quiz = new QuizDto

            {

                QuizId = quizId,

                NameOfQuiz = request.NameOfQuiz,

                Duration = request.Duration,

                PassMark = request.PassMark,

                AttemptsAllowed = request.AttemptsAllowed,

                CreatedBy = createdBy,

                CreatedAt = createdAt

            };

            _quizService.CreateQuiz(quiz, request.TopicId);

            return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, new

            {

                quiz.NameOfQuiz,

                quiz.Duration,

                quiz.PassMark,

                quiz.AttemptsAllowed

            });

        }

        [HttpPut("{id}")]

        public ActionResult UpdateQuiz(Guid id, [FromBody] UpdateQuizDto request)

        {

            var existingQuiz = _quizService.GetQuizById(id);

            if (existingQuiz == null)

                throw new Exception($"Quiz with id {id} not found.");

            if (request.AttemptsAllowed.HasValue && request.AttemptsAllowed <= 0)

                throw new Exception("AttemptsAllowed must be null or a positive integer.");

            if (string.IsNullOrWhiteSpace(request.NameOfQuiz))

                throw new Exception("NameOfQuiz cannot be null or empty.");

            if (request.Duration <= 0)

                throw new Exception("Duration must be a positive integer.");

            if (request.PassMark <= 0)

                throw new Exception("PassMark must be a positive integer.");

            existingQuiz.NameOfQuiz = request.NameOfQuiz;

            existingQuiz.Duration = request.Duration;

            existingQuiz.PassMark = request.PassMark;

            existingQuiz.AttemptsAllowed = request.AttemptsAllowed;

            _quizService.UpdateQuiz(existingQuiz);

            return NoContent();

        }

        [HttpGet("topic/{topicId}")]

        public ActionResult<Guid?> GetQuizIdByTopicId(Guid topicId)

        {

            var quizId = _quizService.GetQuizIdByTopicId(topicId);

            if (quizId == null)

            {

                return Ok(null);

            }

            else

            {

                return Ok(quizId);

            }

        }


        [HttpDelete("{id}")]

        public ActionResult DeleteQuiz(Guid id)

        {

            var existingQuiz = _quizService.GetQuizById(id);

            if (existingQuiz == null)

                throw new Exception($"Quiz with id {id} not found.");

            // Delete related feedback questions first

            _quizFeedbackService.DeleteFeedbackQuestionsByQuizId(id);

            // Then delete the main quiz record

            _quizService.DeleteQuiz(id);

            return NoContent();

        }

    }

}

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using LXP.Core.IServices;
//using LXP.Common.DTO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using LXP.Core.Services;

//namespace LXP.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class QuizController : ControllerBase
//    {
//        private readonly IQuizService _quizService;

//        public QuizController(IQuizService quizService)
//        {
//            _quizService = quizService;
//        }

//        [HttpGet("{id}")]
//        public ActionResult<QuizDto> GetQuizById(Guid id)
//        {
//            var quiz = _quizService.GetQuizById(id);
//            if (quiz == null)
//                throw new Exception($"Quiz with id {id} not found.");

//            var quizResponse = new
//            {
//                quiz.QuizId,
//                quiz.NameOfQuiz,
//                quiz.Duration,
//                quiz.PassMark,
//                quiz.AttemptsAllowed // Include AttemptsAllowed in the response
//            };

//            return Ok(quizResponse);
//        }

//        [HttpGet]
//        public ActionResult<IEnumerable<QuizDto>> GetAllQuizzes()
//        {
//            var quizzes = _quizService.GetAllQuizzes();
//            var quizResponse = quizzes.Select(quiz => new
//            {
//                quiz.QuizId,
//                quiz.NameOfQuiz,
//                quiz.Duration,
//                quiz.PassMark,
//                quiz.AttemptsAllowed // Include AttemptsAllowed in the response
//            });

//            return Ok(quizResponse);
//        }

//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        public ActionResult CreateQuiz([FromBody] CreateQuizDto request)
//        {
//            var quizId = Guid.NewGuid(); // Generate QuizId
//            var createdBy = "System"; // Set createdBy
//            var createdAt = DateTime.UtcNow; // Set createdAt

//            var quiz = new QuizDto
//            {
//                QuizId = quizId,
//                NameOfQuiz = request.NameOfQuiz,
//                Duration = request.Duration,
//                PassMark = request.PassMark,
//                AttemptsAllowed = request.AttemptsAllowed,
//                CreatedBy = createdBy,
//                CreatedAt = createdAt
//            };

//            _quizService.CreateQuiz(quiz, request.TopicId);

//            return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, new
//            {
//                quiz.NameOfQuiz,
//                quiz.Duration,
//                quiz.PassMark,
//                quiz.AttemptsAllowed
//            });
//        }

//        [HttpPut("{id}")]
//        public ActionResult UpdateQuiz(Guid id, [FromBody] UpdateQuizDto request)
//        {
//            var existingQuiz = _quizService.GetQuizById(id);
//            if (existingQuiz == null)
//                throw new Exception($"Quiz with id {id} not found.");

//            if (request.AttemptsAllowed.HasValue && request.AttemptsAllowed <= 0)
//                throw new Exception("AttemptsAllowed must be null or a positive integer.");

//            if (string.IsNullOrWhiteSpace(request.NameOfQuiz))
//                throw new Exception("NameOfQuiz cannot be null or empty.");

//            if (request.Duration <= 0)
//                throw new Exception("Duration must be a positive integer.");

//            if (request.PassMark <= 0)
//                throw new Exception("PassMark must be a positive integer.");

//            existingQuiz.NameOfQuiz = request.NameOfQuiz;
//            existingQuiz.Duration = request.Duration;
//            existingQuiz.PassMark = request.PassMark;
//            existingQuiz.AttemptsAllowed = request.AttemptsAllowed;

//            _quizService.UpdateQuiz(existingQuiz);

//            return NoContent();
//        }

//        [HttpGet("topic/{topicId}")]
//        public ActionResult<Guid?> GetQuizIdByTopicId(Guid topicId)
//        {
//            var quizId = _quizService.GetQuizIdByTopicId(topicId);
//            if (quizId == null)
//            {
//                return Ok(null);
//            }
//            else
//            {
//                return Ok(quizId);
//            }
//        }


//        [HttpDelete("{id}")]
//        public ActionResult DeleteQuiz(Guid id)
//        {
//            var existingQuiz = _quizService.GetQuizById(id);
//            if (existingQuiz == null)
//                throw new Exception($"Quiz with id {id} not found.");

//            _quizService.DeleteQuiz(id);
//            return NoContent();
//        }
//    }
//}





////[HttpGet("topic/{topicId}")]
////public ActionResult<IEnumerable<Guid>> GetQuizzesByTopicId(Guid topicId)
////{
////    var quizzes = _quizService.GetQuizzesByTopicId(topicId);
////    var quizIds = quizzes.Select(quiz => quiz.QuizId).ToList();

////    return Ok(quizIds);
////}


////[HttpGet("topic/{topicId}")]
////public ActionResult<IEnumerable<QuizDto>> GetQuizzesByTopicId(Guid topicId)
////{
////    var quizzes = _quizService.GetQuizzesByTopicId(topicId);
////    var quizResponse = quizzes.Select(quiz => new
////    {
////        quiz.QuizId,
////        //quiz.NameOfQuiz,
////        //quiz.Duration,
////        //quiz.PassMark,
////        //quiz.AttemptsAllowed
////    });

////    return Ok(quizResponse);
////}


////using Microsoft.AspNetCore.Http;
////using Microsoft.AspNetCore.Mvc;
////using LXP.Core.IServices;
////using LXP.Common.DTO;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;

////namespace LXP.Api.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class QuizController : ControllerBase
////    {
////        private readonly IQuizService _quizService;

////        public QuizController(IQuizService quizService)
////        {
////            _quizService = quizService;
////        }

////        [HttpGet("{id}")]
////        public async Task<ActionResult<QuizDto>> GetQuizById(Guid id)
////        {
////            var quiz = await _quizService.GetQuizByIdAsync(id);
////            if (quiz == null)
////                return NotFound($"Quiz with id {id} not found.");

////            return Ok(quiz);
////        }

////        [HttpGet]
////        public async Task<ActionResult<IEnumerable<QuizDto>>> GetAllQuizzes()
////        {
////            var quizzes = await _quizService.GetAllQuizzesAsync();
////            return Ok(quizzes);
////        }

////        [HttpPost]
////        [ProducesResponseType(StatusCodes.Status201Created)]
////        public async Task<ActionResult> CreateQuiz([FromBody] CreateQuizDto request)
////        {
////            if (!ModelState.IsValid)
////                return BadRequest(ModelState);

////            var quizId = Guid.NewGuid();
////            var createdBy = "System";
////            var createdAt = DateTime.UtcNow;

////            var quiz = new QuizDto
////            {
////                QuizId = quizId,
////                NameOfQuiz = request.NameOfQuiz,
////                Duration = request.Duration,
////                PassMark = request.PassMark,
////                AttemptsAllowed = request.AttemptsAllowed,
////                CreatedBy = createdBy,
////                CreatedAt = createdAt
////            };

////            await _quizService.CreateQuizAsync(quiz, request.TopicId);

////            return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, quiz);
////        }

////        [HttpPut("{id}")]
////        public async Task<ActionResult> UpdateQuiz(Guid id, [FromBody] UpdateQuizDto request)
////        {
////            if (!ModelState.IsValid)
////                return BadRequest(ModelState);

////            var existingQuiz = await _quizService.GetQuizByIdAsync(id);
////            if (existingQuiz == null)
////                return NotFound($"Quiz with id {id} not found.");

////            existingQuiz.NameOfQuiz = request.NameOfQuiz;
////            existingQuiz.Duration = request.Duration;
////            existingQuiz.PassMark = request.PassMark;
////            existingQuiz.AttemptsAllowed = request.AttemptsAllowed;

////            await _quizService.UpdateQuizAsync(existingQuiz);

////            return NoContent();
////        }

////        [HttpDelete("{id}")]
////        public async Task<ActionResult> DeleteQuiz(Guid id)
////        {
////            var existingQuiz = await _quizService.GetQuizByIdAsync(id);
////            if (existingQuiz == null)
////                return NotFound($"Quiz with id {id} not found.");

////            await _quizService.DeleteQuizAsync(id);
////            return NoContent();
////        }
////    }
////}


////using Microsoft.AspNetCore.Http;
////using Microsoft.AspNetCore.Mvc;
////using LXP.Core.IServices;
////using LXP.Common.DTO;
////using LXP.Core.Services;
////using LXP.Data;
////using Microsoft.AspNetCore.Http.HttpResults;
////using Org.BouncyCastle.Asn1.Ocsp;
////using LXP.Data.DBContexts;

////namespace LXP.Api.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class QuizController : ControllerBase
////    {
////        private readonly IQuizService _quizService;

////        public QuizController(IQuizService quizService)
////        {
////            _quizService = quizService;
////        }



////        [HttpGet("{id}")]
////        public ActionResult<QuizDto> GetQuizById(Guid id)
////        {
////            var quiz = _quizService.GetQuizById(id);
////            if (quiz == null)
////                throw new Exception($"Quiz with id {id} not found.");

////            var quizResponse = new
////            {
////                quiz.QuizId,
////                quiz.NameOfQuiz,
////                quiz.Duration,
////                quiz.PassMark,
////                quiz.AttemptsAllowed // Include AttemptsAllowed in the response
////            };

////            return Ok(quizResponse);
////        }

////        [HttpGet]
////        public ActionResult<IEnumerable<QuizDto>> GetAllQuizzes()
////        {
////            var quizzes = _quizService.GetAllQuizzes();
////            var quizResponse = quizzes.Select(quiz => new
////            {
////                quiz.QuizId,
////                quiz.NameOfQuiz,
////                quiz.Duration,
////                quiz.PassMark,
////                quiz.AttemptsAllowed // Include AttemptsAllowed in the response
////            });

////            return Ok(quizResponse);
////        }


////        [HttpPost]
////        [ProducesResponseType(StatusCodes.Status201Created)]
////        public ActionResult CreateQuiz([FromBody] CreateQuizDto request)
////        {
////            var quizId = Guid.NewGuid(); // Generate QuizId
////            var createdBy = "System"; // Set createdBy
////            var createdAt = DateTime.UtcNow; // Set createdAt

////            var quiz = new QuizDto
////            {
////                QuizId = quizId,
////                NameOfQuiz = request.NameOfQuiz,
////                Duration = request.Duration,
////                PassMark = request.PassMark,
////                AttemptsAllowed = request.AttemptsAllowed,
////                CreatedBy = createdBy,
////                CreatedAt = createdAt
////            };

////            _quizService.CreateQuiz(quiz, request.TopicId);

////            return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, new { quiz.NameOfQuiz, quiz.Duration, quiz.PassMark, quiz.AttemptsAllowed });
////        }



////        [HttpPut("{id}")]
////        public ActionResult UpdateQuiz(Guid id, [FromBody] UpdateQuizDto request)
////        {
////            // Validate quiz existence
////            var existingQuiz = _quizService.GetQuizById(id);
////            if (existingQuiz == null)
////                throw new Exception($"Quiz with id {id} not found.");

////            // Validate AttemptsAllowed
////            if (request.AttemptsAllowed.HasValue && request.AttemptsAllowed <= 0)
////                throw new Exception("AttemptsAllowed must be null or a positive integer.");

////            // Validate NameOfQuiz
////            if (string.IsNullOrWhiteSpace(request.NameOfQuiz))
////                throw new Exception("NameOfQuiz cannot be null or empty.");

////            // Validate Duration
////            if (request.Duration <= 0)
////                throw new Exception("Duration must be a positive integer.");

////            // Validate PassMark
////            if (request.PassMark <= 0)
////                throw new Exception("PassMark must be a positive integer.");

////            // Update only the allowed fields
////            existingQuiz.NameOfQuiz = request.NameOfQuiz;
////            existingQuiz.Duration = request.Duration;
////            existingQuiz.PassMark = request.PassMark;
////            existingQuiz.AttemptsAllowed = request.AttemptsAllowed; // Assign AttemptsAllowed

////            // Call the service method to update the quiz
////            _quizService.UpdateQuiz(existingQuiz);

////            // Return NoContent if successful
////            return NoContent();
////        }
////        [HttpDelete("{id}")]
////        public ActionResult DeleteQuiz(Guid id)
////        {
////            // Validate quiz existence
////            var existingQuiz = _quizService.GetQuizById(id);
////            if (existingQuiz == null)
////                throw new Exception($"Quiz with id {id} not found.");

////            _quizService.DeleteQuiz(id);
////            return NoContent();
////        }
////    }
////}


////[HttpPost]
////[ProducesResponseType(StatusCodes.Status201Created)]
////public ActionResult CreateQuiz([FromBody] CreateQuizDto request)
////{
////    var quizId = Guid.NewGuid(); // Generate QuizId
////    var courseId = Guid.Parse("a45c9ac3-8e24-4d98-804b-cba8ff59a140"); // Hardcoded CourseId
////    var topicId = Guid.Parse("7ae6ad72-74ef-4e5c-b7d6-5af9dd13d721"); // Hardcoded TopicId
////    var createdBy = "System"; // Set createdBy
////    var createdAt = DateTime.UtcNow; // Set createdAt

////    // Validate AttemptsAllowed
////    if (request.AttemptsAllowed.HasValue && request.AttemptsAllowed <= 0)
////        throw new Exception("AttemptsAllowed must be null or a positive integer.");

////    // Validate NameOfQuiz
////    if (string.IsNullOrWhiteSpace(request.NameOfQuiz))
////        throw new Exception("NameOfQuiz cannot be null or empty.");

////    // Validate Duration
////    if (request.Duration <= 0)
////        throw new Exception("Duration must be a positive integer.");

////    // Validate PassMark
////    if (request.PassMark <= 0)
////        throw new Exception("PassMark must be a positive integer.");

////    var quiz = new QuizDto
////    {
////        QuizId = quizId,
////        CourseId = courseId,
////        TopicId = topicId,
////        NameOfQuiz = request.NameOfQuiz,
////        Duration = request.Duration,
////        PassMark = request.PassMark,
////        AttemptsAllowed = request.AttemptsAllowed, // Assign AttemptsAllowed
////        CreatedBy = createdBy,
////        CreatedAt = createdAt
////    };

////    // Pass the necessary fields to the service
////    _quizService.CreateQuiz(quiz);

////    // Return 201 Created status with the newly created quiz
////    return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, new { quiz.NameOfQuiz, quiz.Duration, quiz.PassMark, quiz.AttemptsAllowed });
////}

////[HttpPost]
////[ProducesResponseType(StatusCodes.Status201Created)]
////public ActionResult CreateQuiz([FromBody] CreateQuizDto request)
////{
////    var quizId = Guid.NewGuid(); // Generate QuizId
////    var createdBy = "System"; // Set createdBy
////    var createdAt = DateTime.UtcNow; // Set createdAt

////    // Fetch the course ID from the topic ID
////    var topic = _dbContext.Topics.FirstOrDefault(t => t.TopicId == request.TopicId);
////    if (topic == null)
////        throw new Exception($"Topic with id {request.TopicId} not found.");

////    var courseId = topic.CourseId;

////    // Validate AttemptsAllowed
////    if (request.AttemptsAllowed.HasValue && request.AttemptsAllowed <= 0)
////        throw new Exception("AttemptsAllowed must be null or a positive integer.");

////    // Validate NameOfQuiz
////    if (string.IsNullOrWhiteSpace(request.NameOfQuiz))
////        throw new Exception("NameOfQuiz cannot be null or empty.");

////    // Validate Duration
////    if (request.Duration <= 0)
////        throw new Exception("Duration must be a positive integer.");

////    // Validate PassMark
////    if (request.PassMark <= 0)
////        throw new Exception("PassMark must be a positive integer.");

////    var quiz = new QuizDto
////    {
////        QuizId = quizId,
////        CourseId = courseId,
////        TopicId = request.TopicId, // Use the TopicId from the request
////        NameOfQuiz = request.NameOfQuiz,
////        Duration = request.Duration,
////        PassMark = request.PassMark,
////        AttemptsAllowed = request.AttemptsAllowed, // Assign AttemptsAllowed
////        CreatedBy = createdBy,
////        CreatedAt = createdAt
////    };

////    // Pass the necessary fields to the service
////    _quizService.CreateQuiz(quiz);

////    // Return 201 Created status with the newly created quiz
////    return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, new { quiz.NameOfQuiz, quiz.Duration, quiz.PassMark, quiz.AttemptsAllowed });
////}

///*  [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        public ActionResult CreateQuiz([FromBody] CreateQuizDto request)
//        {
//            // Generate QuizId
//            var quizId = Guid.NewGuid();

//            // Validate CourseId
//            if (request.CourseId == Guid.Empty)
//                throw new Exception("CourseId cannot be empty.");

//            // Validate TopicId
//            if (request.TopicId == Guid.Empty)
//                throw new Exception("TopicId cannot be empty.");

//            // Validate AttemptsAllowed
//            if (request.AttemptsAllowed.HasValue && request.AttemptsAllowed <= 0)
//                throw new Exception("AttemptsAllowed must be null or a positive integer.");

//            // Validate NameOfQuiz
//            if (string.IsNullOrWhiteSpace(request.NameOfQuiz))
//                throw new Exception("NameOfQuiz cannot be null or empty.");

//            // Validate Duration
//            if (request.Duration <= 0)
//                throw new Exception("Duration must be a positive integer.");

//            // Validate PassMark
//            if (request.PassMark <= 0)
//                throw new Exception("PassMark must be a positive integer.");

//            var createdBy = "System"; // Set createdBy
//            var createdAt = DateTime.UtcNow; // Set createdAt

//            var quiz = new QuizDto
//            {
//                QuizId = quizId,
//                CourseId = request.CourseId, // Assign CourseId from request
//                TopicId = request.TopicId, // Assign TopicId from request
//                NameOfQuiz = request.NameOfQuiz,
//                Duration = request.Duration,
//                PassMark = request.PassMark,
//                AttemptsAllowed = request.AttemptsAllowed, // Assign AttemptsAllowed
//                CreatedBy = createdBy,
//                CreatedAt = createdAt
//            };

//            // Pass the necessary fields to the service
//            _quizService.CreateQuiz(quiz);

//            // Return 201 Created status with the newly created quiz
//            return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, new { quiz.NameOfQuiz, quiz.Duration, quiz.PassMark, quiz.AttemptsAllowed });
//        }
// * 
// * 
// * 
// * 
// */

////[HttpPost]
////[ProducesResponseType(StatusCodes.Status201Created)]
////public ActionResult CreateQuiz([FromBody] CreateQuizDto request)
////{
////    var quizId = Guid.NewGuid(); // Generate QuizId
////    var courseId = Guid.Parse("a45c9ac3-8e24-4d98-804b-cba8ff59a140"); // Hardcoded CourseId
////    var topicId = Guid.Parse("7ae6ad72-74ef-4e5c-b7d6-5af9dd13d721"); // Hardcoded TopicId
////    var createdBy = "System"; // Set createdBy
////    var createdAt = DateTime.UtcNow; // Set createdAt

////    // Validate AttemptsAllowed
////    if (request.AttemptsAllowed.HasValue && request.AttemptsAllowed <= 0)
////        throw new Exception("AttemptsAllowed must be null or a positive integer.");

////    // Validate NameOfQuiz
////    if (string.IsNullOrWhiteSpace(request.NameOfQuiz))
////        throw new Exception("NameOfQuiz cannot be null or empty.");

////    // Validate Duration
////    if (request.Duration <= 0)
////        throw new Exception("Duration must be a positive integer.");

////    // Validate PassMark
////    if (request.PassMark <= 0)
////        throw new Exception("PassMark must be a positive integer.");

////    var quiz = new QuizDto
////    {
////        QuizId = quizId,
////        CourseId = courseId,
////        TopicId = topicId,
////        NameOfQuiz = request.NameOfQuiz,
////        Duration = request.Duration,
////        PassMark = request.PassMark,
////        AttemptsAllowed = request.AttemptsAllowed, // Assign AttemptsAllowed
////        CreatedBy = createdBy,
////        CreatedAt = createdAt
////    };

////    // Pass the necessary fields to the service
////    _quizService.CreateQuiz(quiz);

////    // Return 201 Created status with the newly created quiz
////    return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, new { quiz.NameOfQuiz, quiz.Duration, quiz.PassMark, quiz.AttemptsAllowed });
////}


////[HttpPost]
////[ProducesResponseType(StatusCodes.Status201Created)]
////public ActionResult CreateQuiz([FromBody] CreateQuizDto request)
////{
////    Validate the request
////    ValidateCreateQuizRequest(request);

////    Generate QuizId and set metadata
////   var quizId = Guid.NewGuid();
////    var createdBy = "System"; // This could come from the authenticated user context
////    var createdAt = DateTime.UtcNow;

////    Create QuizDto object
////   var quiz = new QuizDto
////   {
////       QuizId = quizId,
////       CourseId = request.CourseId,
////       TopicId = request.TopicId,
////       NameOfQuiz = request.NameOfQuiz,
////       Duration = request.Duration,
////       PassMark = request.PassMark,
////       AttemptsAllowed = request.AttemptsAllowed,
////       CreatedBy = createdBy,
////       CreatedAt = createdAt
////   };

////    Pass the necessary fields to the service
////    _quizService.CreateQuiz(quiz);

////    Return 201 Created status with the newly created quiz
////    return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, new { quiz.NameOfQuiz, quiz.Duration, quiz.PassMark, quiz.AttemptsAllowed });
////}

////Extracted validation logic into a separate method
////private void ValidateCreateQuizRequest(CreateQuizDto request)
////{
////    if (request == null)
////        throw new ArgumentException("Request cannot be null.");

////    if (request.AttemptsAllowed.HasValue && request.AttemptsAllowed <= 0)
////        throw new ArgumentException("AttemptsAllowed must be null or a positive integer.");

////    if (string.IsNullOrWhiteSpace(request.NameOfQuiz))
////        throw new ArgumentException("NameOfQuiz cannot be null or empty.");

////    if (request.Duration <= 0)
////        throw new ArgumentException("Duration must be a positive integer.");

////    if (request.PassMark <= 0)
////        throw new ArgumentException("PassMark must be a positive integer.");

////    if (request.CourseId == Guid.Empty)
////        throw new ArgumentException("CourseId is required.");

////    if (request.TopicId == Guid.Empty)
////        throw new ArgumentException("TopicId is required.");
////}






////using Microsoft.AspNetCore.Http;
////using Microsoft.AspNetCore.Mvc;
////using LXP.Core.IServices;
////using LXP.Common.DTO;


////namespace LXP.Api.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class QuizController : ControllerBase
////    {
////        private readonly IQuizService _quizService;

////        public QuizController(IQuizService quizService)
////        {
////            _quizService = quizService;
////        }


////        [HttpGet("{id}")]
////        public ActionResult<QuizDto> GetQuizById(Guid id)
////        {
////            var quiz = _quizService.GetQuizById(id);
////            //if (quiz == null)
////            //    return NotFound();

////            // Validate quiz existence
////            if (quiz == null)
////                throw new Exception($"Quiz with id {id} not found.");

////            var quizResponse = new
////            {
////                quiz.QuizId,
////                quiz.NameOfQuiz,
////                quiz.Duration,
////                quiz.PassMark
////            };

////            return Ok(quizResponse);
////        }


////        [HttpGet]
////        public ActionResult<IEnumerable<QuizDto>> GetAllQuizzes()
////        {
////            var quizzes = _quizService.GetAllQuizzes();
////            var quizResponse = quizzes.Select(quiz => new
////            {
////                quiz.QuizId,
////                quiz.NameOfQuiz,
////                quiz.Duration,
////                quiz.PassMark
////            });

////            return Ok(quizResponse);
////        }




////        [HttpPost]
////        [ProducesResponseType(StatusCodes.Status201Created)]
////        public ActionResult CreateQuiz([FromBody] CreateQuizDto request)
////        {
////            var quizId = Guid.NewGuid(); // Generate QuizId
////            var courseId = Guid.Parse("ce753ccb-408c-4d8c-8acd-cbc8c5adcbb8"); // Hardcoded CourseId
////            var topicId = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996"); // Hardcoded TopicId
////            var createdBy = "System"; // Set createdBy
////            var createdAt = DateTime.UtcNow; // Set createdAt

////            var quiz = new QuizDto
////            {
////                QuizId = quizId,
////                CourseId = courseId,
////                TopicId = topicId,
////                NameOfQuiz = request.NameOfQuiz,
////                Duration = request.Duration,
////                PassMark = request.PassMark,
////                CreatedBy = createdBy,
////                CreatedAt = createdAt
////            };

////            // Pass the necessary fields to the service
////            _quizService.CreateQuiz(quiz);

////            // Return 201 Created status with the newly created quiz
////            return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, new { quiz.NameOfQuiz, quiz.Duration, quiz.PassMark });
////        }


////        [HttpPut("{id}")]
////        public ActionResult UpdateQuiz(Guid id, [FromBody] UpdateQuizDto request)
////        {
////            // Validate quiz existence
////            var existingQuiz = _quizService.GetQuizById(id);
////            if (existingQuiz == null)
////                throw new Exception($"Quiz with id {id} not found.");

////            // Validate NameOfQuiz
////            if (string.IsNullOrWhiteSpace(request.NameOfQuiz))
////                throw new Exception("NameOfQuiz cannot be null or empty.");

////            // Validate Duration
////            if (!int.TryParse(request.Duration.ToString(), out int durationValue) || durationValue <= 0)
////                throw new Exception("Duration must be a positive integer.");

////            // Validate PassMark
////            if (!int.TryParse(request.PassMark.ToString(), out int passMarkValue) || passMarkValue <= 0)
////                throw new Exception("PassMark must be a positive integer.");

////            // Update only the allowed fields
////            existingQuiz.NameOfQuiz = request.NameOfQuiz;
////            existingQuiz.Duration = durationValue;
////            existingQuiz.PassMark = passMarkValue;

////            // Call the service method to update the quiz
////            _quizService.UpdateQuiz(existingQuiz);

////            // Return NoContent if successful
////            return NoContent();
////        }


////        [HttpDelete("{id}")]
////        public ActionResult DeleteQuiz(Guid id)
////        {
////            // Validate quiz existence
////            var existingQuiz = _quizService.GetQuizById(id);
////            if (existingQuiz == null)
////                throw new Exception($"Quiz with id {id} not found.");

////            _quizService.DeleteQuiz(id);
////            return NoContent();
////        }
////    }
////}






////[HttpGet("{id}")]
////public ActionResult<QuizDto> GetQuizById(Guid id)
////{
////    var quiz = _quizService.GetQuizById(id);
////    if (quiz == null)
////        return NotFound();

////    return Ok(quiz);
////}

////[HttpGet]
////public ActionResult<IEnumerable<QuizDto>> GetAllQuizzes()
////{
////    var quizzes = _quizService.GetAllQuizzes();
////    return Ok(quizzes);
////}

////[HttpPut("{id}")]
////public ActionResult UpdateQuiz(Guid id, [FromBody] QuizDto request)
////{
////    // Check if the provided ID matches the ID in the request body
////    if (id != request.QuizId)
////        return BadRequest();

////    // Retrieve the existing quiz by ID
////    var existingQuiz = _quizService.GetQuizById(id);
////    if (existingQuiz == null)
////        return NotFound();

////    // Update only the allowed fields
////    existingQuiz.NameOfQuiz = request.NameOfQuiz;
////    existingQuiz.Duration = request.Duration;
////    existingQuiz.PassMark = request.PassMark;

////    // Call the service method to update the quiz
////    _quizService.UpdateQuiz(existingQuiz);

////    // Return NoContent if successful
////    return NoContent();
////}
////[HttpPost]
////[ProducesResponseType(StatusCodes.Status201Created)]
////public ActionResult CreateQuiz([FromBody] QuizDto request)
////{
////    var quizId = Guid.NewGuid(); // Generate QuizId
////    var courseId = Guid.Parse("ce753ccb-408c-4d8c-8acd-cbc8c5adcbb8"); // Hardcoded CourseId
////    var topicId = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996"); // Hardcoded TopicId
////    var createdBy = "System"; // Set createdBy
////    var createdAt = DateTime.UtcNow; // Set createdAt

////    var quiz = new QuizDto
////    {
////        NameOfQuiz = request.NameOfQuiz,
////        Duration = request.Duration,
////        PassMark = request.PassMark,
////    };

////    // Pass the necessary fields to the service
////    _quizService.CreateQuiz(quizId, courseId, topicId, quiz.NameOfQuiz, quiz.Duration, quiz.PassMark, createdBy, createdAt);

////    // Return 201 Created status with the newly created quiz
////    return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, quiz);
////}
//// [HttpPost]
//// public ActionResult CreateQuiz(QuizDto quiz)
//// {
////     _quizService.CreateQuiz(quiz);
////     return CreatedAtAction(nameof(GetQuizById), new { id = quiz.QuizId }, quiz);
//// }

//// [HttpPost]
//// public ActionResult CreateQuiz(QuizDto quiz)
//// {
////     // Generate QuizId
////     quiz.QuizId = Guid.NewGuid();

////     // Hardcoded values for TopicId and CourseId
////     quiz.CourseId = Guid.Parse("ce753ccb-408c-4d8c-8acd-cbc8c5adcbb8");
////     quiz.TopicId = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996");

////     _quizService.CreateQuiz(quiz);
////     return CreatedAtAction(nameof(GetQuizById), new { id = quiz.QuizId }, quiz);
//// }
//// [HttpPut("{id}")]
//// public ActionResult UpdateQuiz(Guid id, QuizDto quiz)
//// {
////     if (id != quiz.QuizId)
////         return BadRequest();

////     _quizService.UpdateQuiz(quiz);
////     return NoContent();
//// }
//// [HttpPost]
//// [ProducesResponseType(StatusCodes.Status201Created)]
//// public ActionResult CreateQuiz([FromBody] QuizDto request)
//// {
////     var quizId = Guid.NewGuid(); // Generate QuizId
////     var courseId = Guid.Parse("ce753ccb-408c-4d8c-8acd-cbc8c5adcbb8"); // Hardcoded CourseId
////     var topicId = Guid.Parse("e3a895e4-1b3f-45b8-9c0a-98f9c0fa4996"); // Hardcoded TopicId
////     var createdBy = "System"; // Set createdBy
////     var createdAt = DateTime.UtcNow; // Set createdAt

////     var quiz = new QuizDto
////     {
////         NameOfQuiz = request.NameOfQuiz,
////         Duration = request.Duration,
////         PassMark = request.PassMark,
////     };

////     // Pass the necessary fields to the service
////     _quizService.CreateQuiz(quizId, courseId, topicId, quiz.NameOfQuiz, quiz.Duration, quiz.PassMark, createdBy, createdAt);

////     // Return 201 Created status with the newly created quiz
////     return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, quiz);
//// }



//// [HttpPut("{id}")]
//// public ActionResult UpdateQuiz(Guid id, [FromBody] QuizDto request)
//// {
////     // Check if the provided ID matches the ID in the request body
////     if (id != request.QuizId)
////         return BadRequest();

////     // Retrieve the existing quiz by ID
////     var existingQuiz = _quizService.GetQuizById(id);
////     if (existingQuiz == null)
////         return NotFound();

////     // Update only the allowed fields
////     existingQuiz.NameOfQuiz = request.NameOfQuiz;
////     existingQuiz.Duration = request.Duration;
////     existingQuiz.PassMark = request.PassMark;

////     // Call the service method to update the quiz
////     _quizService.UpdateQuiz(existingQuiz);

////     // Return NoContent if successful
////     return NoContent();
//// }