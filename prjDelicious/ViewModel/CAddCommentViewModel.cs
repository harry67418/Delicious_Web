using Microsoft.AspNetCore.Http;
using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CAddCommentViewModel
    {
        public TcommentSection _comment { get; set; }
        public CAddCommentViewModel()
        {
            _comment = new TcommentSection();
        }
        public int RecipeID 
        {
            get { return _comment.RecipeId; }
            set { _comment.RecipeId = value; }
        }
        public int MemberID
        {
            get { return _comment.MemberId; }
            set { _comment.MemberId = value; }
        }
        public int CommentID
        {
            get { return _comment.CommentId; }
            set { _comment.CommentId = value; }
        }
        public string Comment
        {
            get { return _comment.Comment; }
            set { _comment.Comment = value; }
        }
        public string Picture
        {
            get { return _comment.Picture; }
            set { _comment.Picture = value; }
        }
        public string Video
        {
            get { return _comment.Video; }
            set { _comment.Video = value; }
        }
        public DateTime PostTime
        {
            get { return _comment.PostTime; }
            set { _comment.PostTime = value; }
        }

        public IFormFile photo { get; set; }
    }
}
