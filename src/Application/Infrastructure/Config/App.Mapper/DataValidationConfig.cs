using MicBeach.Develop.DataValidation;
using MicBeach.DataValidation.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Mapper
{
    /// <summary>
    /// 数据验证配置
    /// </summary>
    public static class DataValidationConfig
    {
        #region 初始化

        public static void Init()
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Config/Validation");
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath).Where(c=>Path.GetExtension(c).Trim('.').ToLower()== "dvconfig").ToArray();
                ValidationConfig.InitFromConfigFile(files);
            }
            //#region Sys

            //#region ViewModel

            //#region 登陆

            ////必填
            //ValidationManager.Required(new ValidationField<LoginViewModel>()
            //{
            //    FieldExpression = c => c.LoginName,
            //    ErrorMessage = "请填写登陆名"
            //}, new ValidationField<LoginViewModel>()
            //{
            //    FieldExpression = c => c.Pwd,
            //    ErrorMessage = "请填写登陆密码"
            //}, new ValidationField<LoginViewModel>()
            //{
            //    FieldExpression = c => c.VerificationCode,
            //    ErrorMessage = "请填写验证码"
            //});
            ////用户名密码长度
            //ValidationManager.StringLength(20, 5, new ValidationField<LoginViewModel>()
            //{
            //    FieldExpression = c => c.LoginName,
            //    ErrorMessage = "请填写正确的登陆名"
            //}, new ValidationField<LoginViewModel>()
            //{
            //    FieldExpression = c => c.Pwd,
            //    ErrorMessage = "请填写正确的登陆密码"
            //});
            ////验证码长度
            //ValidationManager.StringLength(20, 5, new ValidationField<LoginViewModel>()
            //{
            //    FieldExpression = c => c.VerificationCode,
            //    ErrorMessage = "请填写正确的验证码"
            //});

            //#endregion

            //#region 修改密码

            ////密码必填
            //ValidationManager.Required(new ValidationField<ModifyPasswordViewModel>()
            //{
            //    FieldExpression = c => c.NowPassword,
            //    ErrorMessage = "请填写当前登录密码"
            //}, new ValidationField<ModifyPasswordViewModel>()
            //{
            //    FieldExpression = c => c.NewPassword,
            //    ErrorMessage = "请填写新的登陆密码"
            //}, new ValidationField<ModifyPasswordViewModel>()
            //{
            //    FieldExpression = c => c.ConfirmNewPassword,
            //    ErrorMessage = "请确认新的密码"
            //});
            ////密码规则
            //ValidationManager.StringLength(20, 5, new ValidationField<ModifyPasswordViewModel>()
            //{
            //    FieldExpression = c => c.NowPassword,
            //    ErrorMessage = "请填写5-20个字符内的正确密码"
            //}, new ValidationField<ModifyPasswordViewModel>()
            //{
            //    FieldExpression = c => c.NewPassword,
            //    ErrorMessage = "请填写5-20个字符内的正确密码"
            //}, new ValidationField<ModifyPasswordViewModel>()
            //{
            //    FieldExpression = c => c.ConfirmNewPassword,
            //    ErrorMessage = "请填写5-20个字符内的正确密码"
            //});
            ////密码比较
            //ValidationManager.Equal(c => c.NewPassword, new ValidationField<ModifyPasswordViewModel>()
            //{
            //    FieldExpression = c => c.ConfirmNewPassword,
            //    ErrorMessage = "两次填写的密码不相同"
            //});
            //#endregion

            //#region 角色

            ////必填
            //ValidationManager.Required(new ValidationField<RoleViewModel>()
            //{
            //    FieldExpression = c => c.Name,
            //    ErrorMessage = "请填写20个字符内的名字",
            //    TipMessage = true

            //},
            //new ValidationField<RoleViewModel>()
            //{
            //    FieldExpression = c => c.Status,
            //    ErrorMessage = "请选择状态"
            //});
            ////名称长度
            //ValidationManager.StringLength(20, 0, new ValidationField<RoleViewModel>()
            //{
            //    FieldExpression = c => c.Name,
            //    ErrorMessage = "请填写20个字符内的名字",
            //});
            ////说明长度
            //ValidationManager.StringLength(20, 0, new ValidationField<RoleViewModel>()
            //{
            //    FieldExpression = c => c.Remark,
            //    ErrorMessage = "请填写50个字符内的说明",
            //    TipMessage = true
            //});

            //#endregion

            //#endregion

            //#region Domain

            //#endregion    
               
            //#endregion
        }

        #endregion
    }
}
