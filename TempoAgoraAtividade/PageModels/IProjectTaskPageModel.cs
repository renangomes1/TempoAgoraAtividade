using CommunityToolkit.Mvvm.Input;
using TempoAgoraAtividade.Models;

namespace TempoAgoraAtividade.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}