using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Common.Utils;
using TeraLinkaCareApi.Core.Domain.Entities;
using TeraLinkaCareApi.Core.Domain.Interfaces;
using TeraLinkaCareApi.Infrastructure.Persistence;
using TeraLinkaCareApi.Infrastructure.Persistence.UnitOfWork.Interfaces;

namespace TeraLinkaCareApi.API.Controllers;

/// <summary>
/// DICOM 影像控制器，處理與 DICOM 影像相關的操作
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DicomImageController : ControllerBase
{
    private readonly ILogger<DicomImageController> _logger;
    private readonly IRepository<DicomImage, CRSDbContext> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string? _imageMarkerDirPath;

    public DicomImageController(
        ILogger<DicomImageController> logger,
        IRepository<DicomImage, CRSDbContext> repository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _imageMarkerDirPath = configuration.GetSection("ImageMarkerDirPath").Value;

        if (!Directory.Exists(_imageMarkerDirPath))
            Directory.CreateDirectory(_imageMarkerDirPath);
    }

    /// <summary>
    /// 更新 DICOM 影像的標記
    /// </summary>
    /// <param name="sopInstanceUid">DICOM 影像的 SOP Instance UID</param>
    /// <param name="imageMarker">影像標記資訊</param>
    /// <returns>更新後的 DICOM 影像資訊</returns>
    [HttpPost("sopInstanceUid/{sopInstanceUid}/imageMarker")]
    public async Task<ActionResult<string>> PostImageMarker(
        [Required(ErrorMessage = "SOP Instance UID 為必填項")]
        string sopInstanceUid,
        [FromBody] ImageMarkerDto imageMarker
    )
    {
        try
        {
            // 驗證輸入
            if (imageMarker == null)
            {
                _logger.LogWarning("請求內容為空");
                return BadRequest("請求內容不能為空");
            }

            var dicomImage = new DicomImage
            {
                SOPInstanceUID = sopInstanceUid,
                ImageMarker = imageMarker.ImageMarker,
                ImageMarkerUrl = ImageUtils.ConvertBase64PngToJpg(imageMarker.ImageMarkerUrl,
                    Path.Combine(_imageMarkerDirPath, sopInstanceUid)),
            };

            await _repository.UpdatePartialAsync(dicomImage, "SOPInstanceUID", "ImageMarker", "ImageMarkerUrl");
            await _unitOfWork.SaveAsync();

            return Ok("更新成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "更新 DICOM 影像標記時發生錯誤，SOP Instance UID: {SopInstanceUid}",
                sopInstanceUid
            );
            return StatusCode(500, "更新 DICOM 影像標記時發生錯誤");
        }
    }

    /// <summary>
    /// 更新 DICOM 影像的說明
    /// </summary>
    /// <param name="sopInstanceUid">DICOM 影像的 SOP Instance UID</param>
    /// <param name="imageComment">影像標記資訊</param>
    /// <returns>更新後的 DICOM 影像資訊</returns>
    [HttpPost("sopInstanceUid/{sopInstanceUid}/imageComment")]
    public async Task<ActionResult<string>> PostImageComment(
        [Required(ErrorMessage = "SOP Instance UID 為必填項")]
        string sopInstanceUid,
        [FromBody] ImageCommentDto imageComment
    )
    {
        try
        {
            var dicomImage = new DicomImage
            {
                SOPInstanceUID = sopInstanceUid,
                ImageComment = imageComment.ImageComment
            };

            await _repository.UpdatePartialAsync(dicomImage, "SOPInstanceUID", "ImageComment");
            await _unitOfWork.SaveAsync();

            return Ok("更新成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新影像說明時發生錯誤，SOP Instance UID: {SopInstanceUid}", sopInstanceUid);
            return StatusCode(500, "更新影像說明時發生錯誤");
        }
    }
}