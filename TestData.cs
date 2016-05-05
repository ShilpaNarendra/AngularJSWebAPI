using InEight.Core.Models;
using InEight.Core.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace InEight.Core.Web.Services.Tests
{
	public static class DataContextUtils
	{
		public static void HookupNavigation<T0, T1, T2, T3>(T0 dbContext, Expression<Func<T0,DbSet<T1>>> source, Expression<Func<T1, T3>> sourceID, Expression<Func<T1, T2>> sourceNav,
			Expression<Func<T0,DbSet<T2>>> dest, Expression<Func<T2, ICollection<T1>>> destNav)
			where T1 : class
			where T2 : class
		{
			if (dbContext is DbContext) return; // we have an actual DbContext, not a mock, so hookups should be done automatically

			var sourceProperty = (PropertyInfo)((MemberExpression)source.Body).Member;
			var sourceDbSet = sourceProperty.GetValue(dbContext) as DbSet<T1>;
			var destProperty = (PropertyInfo)((MemberExpression)dest.Body).Member;
			var destDbSet = destProperty.GetValue(dbContext) as DbSet<T2>;

			if (destNav != null)
				foreach (var t2 in destDbSet)
				{
					var navProperty = (PropertyInfo)((MemberExpression)destNav.Body).Member;
					navProperty.SetValue(t2, new List<T1>());
				}

			foreach (var t1 in sourceDbSet)
			{
				var idProperty = (PropertyInfo)((MemberExpression)sourceID.Body).Member;
				var navProperty = (PropertyInfo)((MemberExpression)sourceNav.Body).Member;
				var id = idProperty.GetValue(t1);
				if (id != null)
				{
					var t2 = destDbSet.Find(id);
					if (t2 != null)
					{
						navProperty.SetValue(t1, t2);

						if (destNav != null)
						{
							var destNavProperty = (PropertyInfo)((MemberExpression)destNav.Body).Member;
							var coll = destNavProperty.GetValue(t2) as List<T1>;
							coll.Add(t1);
						}
					}
				}
			}
		}
	}

	public class TestConfigurationDataContext : IConfigurationDataContext
	{
		public TestConfigurationDataContext()
		{
			this.ReSellers = new TestReSellerDbSet(this);
			this.Accounts = new TestAccountDbSet(this);
			this.Applications = new TestApplicationDbSet(this);
			this.AdminLevels = new TestAdminLevelDbSet(this);
			this.ActionGroups = new TestActionGroupDbSet(this);
			this.Actions = new TestActionDbSet(this);
			this.ActionItemDependencies = new TestActionItemDependencyDbSet(this);
			this.AppLicenses = new TestAppLicenseDbSet(this);
		}

		public DbSet<AccessType> AccessTypes { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<AdminLevel> AdminLevels { get; set; }
		public DbSet<InEight.Core.Models.Action> Actions { get; set; }
		public DbSet<ActionItemDependency> ActionItemDependencies { get; set; }
		public DbSet<ActionGroup> ActionGroups { get; set; }
		public DbSet<Application> Applications { get; set; }
		public DbSet<AppLicense> AppLicenses { get; set; }
		public DbSet<EquipmentQualificationAssn> EquipmentQualificationAssns { get; set; }
		public DbSet<HostType> HostTypes { get; set; }
		public DbSet<ReSeller> ReSellers { get; set; }
		public DbSet<ServerAccessInfo> ServerAccessInfoes { get; set; }
		public DbSet<ServerInfo> ServerInfoes { get; set; }
		public DbSet<SMPBackendConnection> SMPBackendConnections { get; set; }
		public DbSet<SMPServerInfo> SMPServerInfoes { get; set; }

		public int SaveChanges() { return 0; }
		public Task<int> SaveChangesAsync() { return Task.Run(() => { return SaveChanges(); }); }

		public void Dispose()
		{
		}

		public DbContextTransaction BeginTransaction()
		{
			return null;
		}
	}

	public class TestCoreDataContext : ICoreDataContext
	{
		public TestCoreDataContext()
		{
			this.AttachmentCategories = new TestAttachmentCategoryDbSet(this);
			this.AttachmentTypes = new TestAttachmentTypesDbSet(this);
			this.CountryMasters = new TestCountryMasterDbSet(this);
			this.Currencies = new TestCurrencyDbSet(this);
			this.EquipmentQualificationAssns = new TestEquipmentQualificationAssnsDbSet(this);
			this.EquipmentTypeQualificationAssns = new TestEquipmentTypeQualificationAssnsDbSet(this);
			this.OrganizationCurrencyAssns = new TestOrganizationCurrencyAssnDbSet(this);
			this.Organizations = new TestOrganizationDbSet(this);
			this.RegionMasters = new TestRegionMasterDbSet(this);
			this.Users = new TestUserDbSet(this);
			this.UserOrganizationAssns = new TestUserOrganizationAssnDbSet(this);
			this.UserRoleAssns = new TestUserRoleAssnDbSet(this);
			this.RolePermissions = new TestRolePermissionDbSet(this);
			this.Roles = new TestRoleDbSet(this);
			this.Equipment = new TestEquipmentDbSet(this);
			this.UserGroupAssns = new TestUserGroupAssnDbSet(this);
			this.MeasurementSystems = new TestMeasurementSystemDbSet(this);
			this.MeasurementTypes = new TestMeasurementTypeDbSet(this);
			this.UOMs = new TestUomDbSet(this);
			this.MeasurementTypeUOMMappings = new TestMeasurementTypeUOMMappingDbSet(this);
            this.Projects = new TestProjectDbSet(this);
		}

		#region // data sets

		public DbSet<CountryMaster> CountryMasters { get; set; }
		public DbSet<Organization> Organizations { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<EquipmentCategory> EquipmentCategories { get; set; }
		public DbSet<EquipmentType> EquipmentTypes { get; set; }
		public DbSet<ProjectUserAssn> ProjectUserAssns { get; set; }
		public DbSet<RegionMaster> RegionMasters { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }
		public DbSet<RoleApplication> RoleApplications { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserCertificationAssn> UserCertificationAssns { get; set; }
		public DbSet<UserGroup> UserGroups { get; set; }
		public DbSet<UserGroupAssn> UserGroupAssns { get; set; }
		public DbSet<UserRoleAssn> UserRoleAssns { get; set; }
		public DbSet<UserOrganizationAssn> UserOrganizationAssns { get; set; }
		public DbSet<Email> Emails { get; set; }
		public DbSet<Enumerate> Enumerates { get; set; }
		public DbSet<OwnershipType> OwnershipTypes { get; set; }
		public DbSet<Equipment> Equipment { get; set; }
		public DbSet<UserPermission> UserPermissions { get; set; }
		public DbSet<LabourCategory> LabourCategories { get; set; }
		public DbSet<AttachmentCategory> AttachmentCategories { get; set; }
		public DbSet<AttachmentType> AttachmentTypes { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<LabourType> LabourTypes { get; set; }
		public DbSet<EquipmentCategoryQualificationAssn> EquipmentCategoryQualificationAssns { get; set; }
		public DbSet<Qualification> Qualifications { get; set; }
		public DbSet<EquipmentQualificationAssn> EquipmentQualificationAssns { get; set; }
		public DbSet<EquipmentTypeQualificationAssn> EquipmentTypeQualificationAssns { get; set; }
		public DbSet<SupportedBrowser> SupportedBrowsers { get; set; }
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<OrganizationCurrencyAssn> OrganizationCurrencyAssns { get; set; }
		public DbSet<ProjectEquipment> ProjectEquipment { get; set; }
		public DbSet<ProjectEmployee> ProjectEmployee { get; set; }
		public DbSet<MeasurementSystem> MeasurementSystems { get; set; }
		public DbSet<MeasurementType> MeasurementTypes { get; set; }
		public DbSet<MeasurementTypeUOMMapping> MeasurementTypeUOMMappings { get; set; }
		public DbSet<UOM> UOMs { get; set; }

		#endregion

		public IQueryable<Organization> GetOrganizationAncestors(Guid organizationId)
		{
			if (organizationId != null)
			{

				var org = from o in Organizations
						  where o.OrganizationID.Equals(organizationId) 
						  select o;
				if (org.Any() && org.FirstOrDefault().ParentID != null)
				{
					var orgparent = from o in Organizations 
									from r in org 
									where (o.OrganizationID.Equals(r.ParentID))
									select o;


					return org.Union(orgparent);
				}
				return org;

			}
			return null;

		}
		public IQueryable<Organization> GetOrganizationTree(Guid organizationId)
		{

			return Organizations.Where(p => p.ParentID.Equals(organizationId) || (p.ParentID == null && p.OrganizationID.Equals(organizationId)));
		}
		public IQueryable<Organization> GetOrganizationTreeForUser(Guid userId)
		{
			var org = from ua in UserOrganizationAssns
					  join o in Organizations on ua.OrganizationID equals o.OrganizationID
					  where ua.UserID.Equals(userId)
					  select o;

			var orgparent = from o in Organizations
							from r in org
							where (o.OrganizationID.Equals(r.ParentID) || r.ParentID == null)
							select o;

			var orgchild = from o in Organizations
						   join r in org on o.OrganizationID equals r.OrganizationID
						   select o;

			return orgparent.Union(orgchild);
		}
		public IQueryable<RolePermission> GetOrganizationPermissions(Guid userId, Guid organizationId)
		{
			var rolePermission = from o in GetOrganizationAncestors(organizationId)
								 join ua in UserOrganizationAssns on o.OrganizationID equals ua.OrganizationID
								 join ur in UserRoleAssns on ua.UserOrganizationAssnID equals ur.UserOrganizationAssnID.Value
								 join r in RolePermissions on ur.RoleID equals r.RoleID
								 where ua.UserID.Equals(userId) && ua.IsActive && ur.IsActive && r.IsActive
								 select r;
			return rolePermission;
		}
		public IQueryable<RolePermission> GetProjectPermissions(Guid userId, Guid projectId) { return null; }
        public bool HasOrganizationPermission(long actionId, Guid userId, Guid organizationId)
        {
            return GetOrganizationPermissions(userId, organizationId).Any(a => a.ActionID == actionId);
        }
		public bool HasProjectPermission(long actionId, Guid userId, Guid projectId) { return true; }
		public IQueryable<RolePermission> GetPermissions(Guid userId) { return null; }
		public bool HasPermission(long actionId, Guid userId) { return true; }

		public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
		{
			return null;
		}

		public void CopyEntityValues<TEntity>(TEntity fromEntity, TEntity toEntity) where TEntity : class { }

		public int SaveChanges() { return 0; }
		public Task<int> SaveChangesAsync() { return Task.Run(() => { return SaveChanges(); }); }

		public DbContextTransaction BeginTransaction()
		{
			return null;
		}

		public void Dispose() { }
	}
}
