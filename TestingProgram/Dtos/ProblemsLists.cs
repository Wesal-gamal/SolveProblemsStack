using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingProgram.Dtos
{
    public class ProblemsListsDto
    {
        public ProblemsListsDto()
        {
            Solutions = new List<SolutionsListsDto>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<SolutionsListsDto> Solutions { get; set; }
    }
    public class SolutionsListsDto
    {
        public SolutionsListsDto()
        {
            comments = new List<CommentsListsDto>();
        }
        public int Id { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int DisLikeCount { get; set; }
        public List<CommentsListsDto> comments { get; set; }
    }
    public class CommentsListsDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int DisLikeCount { get; set; }
    }
    }
