using prjDelicious.Repositories;
using prjDelicious.Services;
using prjDelicious.ViewModel;
using System;
using System.Linq;

namespace prjDelicious.Models
{
    public class CFolderPageService : IFolderPageService
    {
        private readonly IFolderPageRepository _folderPageRepository;
        public CFolderPageService(IFolderPageRepository folderPageRepository)
        {
            _folderPageRepository = folderPageRepository;
        }

        //查詢此會員所有收藏夾和收藏食譜
        public CFolderPageViewModel QueryAllMemberFolderAndRecipes(int memberId)
        {
            if (memberId == 0) throw new Exception("查無此會員");
            
            CFolderPageViewModel model = new CFolderPageViewModel();
            model.recipes = _folderPageRepository.QueryAllMemberCollectionRecipe(memberId);
            model.folders = _folderPageRepository.QueryAllMemberFolder(memberId);

            return model;
        }
        //查詢收藏夾內所有食譜
        public CFolderPageViewModel QueryRecipeInFolder(int memberId, int folderId)
        {
            if (memberId == 0) throw new Exception("查無此會員");
            if (folderId == 0) throw new Exception("發生錯誤,無此收藏夾");

            CFolderPageViewModel model = new CFolderPageViewModel();
            var folders = _folderPageRepository.QueryAllMemberFolder(memberId);
            var reciprs = _folderPageRepository.QueryRecipeInFolder(memberId, folderId);

            model.folders = folders.ToList();
            model.recipes = reciprs.ToList();

            return model;
        }
        //查詢該食譜的收藏夾
        public IQueryable QueryFolderWithRecipe(int memberId, int recipeId)
        {
            if (memberId == 0) throw new Exception("查無此會員");
            if (recipeId == 0) throw new Exception("發生錯誤,此食譜已被刪除或檢舉");

            var folder = _folderPageRepository.QueryFolderWithRecipe(memberId, recipeId);
            return folder;
        }

        //食譜加入收藏夾
        public bool InsertRecipeInFolder(int recipeId, int folderId)
        {
            if (folderId == 0) throw new Exception("發生錯誤,無此收藏夾");
            if (recipeId == 0) throw new Exception("發生錯誤,此食譜已被刪除或檢舉");

            Tcollection tcollection = new Tcollection();
            tcollection.CollectionFolderId = folderId;
            tcollection.ReicipeId = recipeId;
            tcollection.Datetime = DateTime.Now.ToLocalTime();
            return _folderPageRepository.InsertRecipeInFolder(tcollection);
        }
        //食譜移出收藏夾
        public bool MoveRecipeOutFolder(int recipeId, int folderId)
        {
            if (folderId == 0) throw new Exception("發生錯誤,無此收藏夾");
            if (recipeId == 0) throw new Exception("發生錯誤,此食譜已被刪除或檢舉");

            return _folderPageRepository.MoveRecipeOutFolder(recipeId,folderId);
        }

        //新增收藏夾
        public bool CreateFolder(string folderName, int memberId)
        {
            if (memberId == 0) throw new Exception("發生錯誤,無登入會員");
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                TcollectionFolder tcollectionFolder = new TcollectionFolder();
                tcollectionFolder.CollectionFolder = folderName;
                tcollectionFolder.MemberId = memberId;

                return _folderPageRepository.CreateFolder(tcollectionFolder);
            }
            else
            {
                throw new AccessViolationException("發生錯誤,請輸入收藏夾名稱");
            }
            
        }
        //編輯收藏夾名稱
        public bool EditFolderName(string folderName , int folderId)
        {
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                folderName = folderName.Trim(' ');
                return _folderPageRepository.EditFolderName(folderName,folderId);
            }
            else 
            {
                throw new Exception("發生錯誤,請輸入收藏夾名稱");
            }
        }
        //刪除收藏夾
        public bool DeleteFolder(int folderId)
        {
            if (folderId == 0) throw new Exception("發生錯誤,無此收藏夾");
            return _folderPageRepository.DeleteFolder(folderId);
        }
    }
}
