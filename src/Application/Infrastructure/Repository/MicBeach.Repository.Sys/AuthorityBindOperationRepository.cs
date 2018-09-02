using MicBeach.Domain.Sys.Model;
using MicBeach.Domain.Sys.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.DataAccessContract.Sys;
using MicBeach.Entity.Sys;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;
using MicBeach.Develop.UnitOfWork;

namespace MicBeach.Repository.Sys
{
    public class AuthorityBindOperationRepository : IAuthorityBindOperationRepository
    {
        IAuthorityBindOperationDataAccess authorityBindOperationDataAccess = null;

        public AuthorityBindOperationRepository()
        {
            authorityBindOperationDataAccess = this.Instance<IAuthorityBindOperationDataAccess>();
        }

        #region 保存授权操作时修改绑定的权限

        public void SaveAuthorityOperation(IEnumerable<AuthorityOperation> operations)
        {
            if (operations.IsNullOrEmpty())
            {
                return;
            }
            List<Tuple<Authority, AuthorityOperation>> binds = new List<Tuple<Authority, AuthorityOperation>>();
            foreach (var operation in operations)
            {
                if (operation == null || operation.Authoritys.IsNullOrEmpty())
                {
                    continue;
                }
                binds.AddRange(operation.Authoritys.Select(c => new Tuple<Authority, AuthorityOperation>(c, operation)));
            }
            Bind(binds);
        }

        #endregion

        #region 删除授权操作时删除绑定的权限

        /// <summary>
        /// 删除授权操作时删除绑定的权限
        /// </summary>
        /// <param name="operations">授权操作</param>
        public void DeleteBindAuthorityByOperation(IEnumerable<AuthorityOperation> operations)
        {
            if (operations.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<long> operationIds = operations.Select(c => c.SysNo).Distinct();
            IQuery query = QueryFactory.Create<AuthorityBindOperationQuery>(c => operationIds.Contains(c.AuthorithOperation));
            UnitOfWork.RegisterCommand(authorityBindOperationDataAccess.Delete(query));
        }

        #endregion

        #region 删除权限数据时删除绑定的权限

        /// <summary>
        /// 删除权限数据时删除绑定的权限
        /// </summary>
        /// <param name="authoritys">权限操作</param>
        public void DeleteBindOperationByAuthority(IEnumerable<Authority> authoritys)
        {
            if (authoritys.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<string> authCodes = authoritys.Select(c => c.Code).Distinct();
            IQuery query = QueryFactory.Create<AuthorityBindOperationQuery>(c => authCodes.Contains(c.AuthorityCode));
            UnitOfWork.RegisterCommand(authorityBindOperationDataAccess.Delete(query));
        }

        #endregion

        #region 权限绑定操作

        /// <summary>
        /// 权限绑定操作
        /// </summary>
        /// <param name="authority">权限数据</param>
        /// <param name="operations">操作信息</param>
        public void Bind(IEnumerable<Tuple<Authority, AuthorityOperation>> binds)
        {
            if (binds.IsNullOrEmpty())
            {
                return;
            }
            List<AuthorityBindOperationEntity> bindEntitys = new List<AuthorityBindOperationEntity>();
            IQuery removeQuery = QueryFactory.Create<AuthorityBindOperationQuery>();
            foreach (var bind in binds)
            {
                if (bind.Item1 == null || bind.Item2 == null)
                {
                    continue;
                }
                removeQuery.Or<AuthorityBindOperationQuery>(c => c.AuthorityCode == bind.Item1.Code && c.AuthorithOperation == bind.Item2.SysNo);
                bindEntitys.Add(new AuthorityBindOperationEntity()
                {
                    AuthorityCode = bind.Item1.Code,
                    AuthorithOperation = bind.Item2.SysNo
                });
            }
            UnitOfWork.RegisterCommand(authorityBindOperationDataAccess.Delete(removeQuery));//移除当前
            UnitOfWork.RegisterCommand(authorityBindOperationDataAccess.Add(bindEntitys).ToArray());//保存
        }

        #endregion

        #region 解绑权限绑定的操作

        /// <summary>
        /// 解绑权限绑定的操作
        /// </summary>
        /// <param name="binds">绑定信息</param>
        public void UnBind(IEnumerable<Tuple<Authority, AuthorityOperation>> binds)
        {
            if (binds.IsNullOrEmpty())
            {
                return;
            }
            IQuery removeQuery = QueryFactory.Create<AuthorityBindOperationQuery>();
            foreach (var bind in binds)
            {
                if (bind.Item1 == null || bind.Item2 == null)
                {
                    continue;
                }
                removeQuery.Or<AuthorityBindOperationQuery>(c => c.AuthorityCode == bind.Item1.Code && c.AuthorithOperation == bind.Item2.SysNo);
            }
            UnitOfWork.RegisterCommand(authorityBindOperationDataAccess.Delete(removeQuery));
        }

        #endregion
    }
}
