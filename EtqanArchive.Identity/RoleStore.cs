using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using DataLayer.Security.TableEntity;
using EtqanArchive.DataLayer;

namespace EtqanArchive.Identity
{
    public class RoleStore : IRoleStore<Role> , IQueryableRoleStore<Role>
    {
        private readonly EtqanArchiveDBContext _context;

        public IQueryable<Role> Roles => _context.Roles;

        public RoleStore(EtqanArchiveDBContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context is null");
            }
            _context = context;
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(nameof(roleId));
            Guid id;
            if (!Guid.TryParse(roleId, out id))
            {
                throw new ArgumentException("Not a valid id", nameof(roleId));
            }
            return await _context.Roles.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));
            return await _context.Roles.SingleOrDefaultAsync(u => u.Name == roleName, cancellationToken);
        }

        public virtual async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            _context.Add(role);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not create role." });
        }

        public virtual async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            var roleFromDb = await _context.Roles.FindAsync(role.Id);
            _context.Remove(roleFromDb);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete role." });
        }

        public virtual async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            _context.Update(role);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not update role." });
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            role.Name = roleName;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            //cancellationToken.ThrowIfCancellationRequested();
            //if (role == null) throw new ArgumentNullException(nameof(role));
            //role.NormalizedName = normalizedName;
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {

        }

    }
}
