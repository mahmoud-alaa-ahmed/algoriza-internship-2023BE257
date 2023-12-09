namespace Vezeeta.Core.Dtos.PatientDto
{
	public class GetPatientByIdDto:GetAllPatientResponseDto
	{
        public List<RequestDto> Requests { get; set; }
    }
}
