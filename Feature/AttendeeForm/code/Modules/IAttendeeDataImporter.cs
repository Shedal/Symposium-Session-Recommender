using Sitecore.XConnect;

namespace Recommender.AttendeeForm.Modules
{
    public interface IAttendeeDataImporter
    {
        void ImportData(IXdbContext client, Contact contact);
    }
}