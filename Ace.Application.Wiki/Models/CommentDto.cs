using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class CommentInputBase : ValidationModel
    {
         

        public string Summary { get; set; }

        public string UserID { get; set; }
        public string Fid { get; set; }
        public DateTime CreateDate { get; set; }



    }


    [MapToType(typeof(Comment))]
    public class AddCommentInput : CommentInputBase
    { 
    }

    [MapToType(typeof(Comment))]
    public class UpdateCommentInput : CommentInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
