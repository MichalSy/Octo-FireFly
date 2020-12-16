using ChartJs.Blazor;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Axes.Ticks;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using Microsoft.Extensions.Logging;
using Octo_FireFly.App.Common;
using Octo_FireFly.App.Manager.Temperature;
using System;
using System.Collections.Generic;
using System.Drawing;
using static Octo_FireFly.App.Common.SampleUtils;

namespace Octo_FireFly.App.Pages.Print.Models
{
    public class PrintPageViewModel : ViewModelBase
    {
        private readonly ILogger<PrintPageViewModel> _logger;
        private readonly ITemperatureManager _temperatureManager;

        public decimal CurrentToolActual { get; set; }

        public decimal CurrentToolTarget { get; set; }

        public decimal CurrentBedActual { get; set; }
        public decimal CurrentBedTarget { get; set; }

        public LineConfig ChartConfig { get; set; }
        public Chart Chart { get; set; }

        public PrintPageViewModel(ILogger<PrintPageViewModel> logger, ITemperatureManager temperatureManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _temperatureManager = temperatureManager ?? throw new ArgumentNullException(nameof(temperatureManager));
        }

        private void OnTemperatureChangedExecute(object sender, EventArgs e)
        {
            CurrentToolActual = _temperatureManager.CurrentToolActual;
            CurrentToolTarget = _temperatureManager.CurrentToolTarget;
            CurrentBedActual = _temperatureManager.CurrentBedActual;
            CurrentBedTarget = _temperatureManager.CurrentBedTarget;

            _logger.LogInformation($"{_temperatureManager.CurrentToolActual}");

            OnUIChanged?.Invoke();

            var datasetTool = (IDataset<decimal>)ChartConfig.Data.Datasets[1];
            var datasetBed = (IDataset<decimal>)ChartConfig.Data.Datasets[0];
            if (datasetTool.Count >= 20)
            {
                datasetTool.RemoveAt(0);
                datasetBed.RemoveAt(0);
            }
            else
            {
                ChartConfig.Data.Labels.Add("");
            }


            _logger.LogInformation($"{datasetTool.Count}");
            datasetTool.Add(_temperatureManager.CurrentToolActual);
            datasetBed.Add(_temperatureManager.CurrentBedActual);

            Chart.Update();
        }

        public override void OnPageShown()
        {
            _logger.LogInformation("SHOWN");
            _temperatureManager.OnTemperatureChanged += OnTemperatureChangedExecute;

            ChartConfig = new LineConfig
            {
                Options = new LineOptions
                {
                    Legend = new Legend
                    {
                        Labels = new LegendLabels
                        {
                            FontColor = ColorUtil.FromDrawingColor(Color.White)
                        }
                    },
                    Responsive = true,
                    Tooltips = new Tooltips
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = true
                    },
                    Hover = new Hover
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = true
                    },
                    Scales = new Scales
                    {
                        XAxes = new List<CartesianAxis>
                        {
                            new CategoryAxis
                            {
                                GridLines = new GridLines
                                {
                                    Color = ColorUtil.FromDrawingColor(Color.White)
                                },
                                Display = AxisDisplay.False,
                                ScaleLabel = new ScaleLabel
                                {
                                    Display = false,
                                    FontColor = ColorUtil.FromDrawingColor(Color.White),
                                    LabelString = "Month"
                                }
                            }
                        },
                        YAxes = new List<CartesianAxis>
                        {
                            new LinearCartesianAxis
                            {
                                GridLines = new GridLines
                                {
                                    DrawTicks = true,
                                    DrawOnChartArea = true,
                                    ZeroLineColor = ColorUtil.FromDrawingColor(Color.White),
                                    Color = ColorUtil.FromDrawingColor(Color.White)
                                },
                                Ticks = new LinearCartesianTicks
                                {
                                    FontColor = ColorUtil.FromDrawingColor(Color.White),
                                    StepSize = 20,
                                    SuggestedMin = 40
                                },
                                ScaleLabel = new ScaleLabel
                                {
                                    Display = false,
                                    FontColor = ColorUtil.FromDrawingColor(Color.White),
                                    LabelString = "Value"
                                }
                            }
                        }
                    }
                }
            };

            IDataset<decimal> dataset1 = new LineDataset<decimal>()
            {
                Label = "Bed",
                BackgroundColor = ColorUtil.FromDrawingColor(ChartColors.Blue),
                BorderColor = ColorUtil.FromDrawingColor(ChartColors.Blue),
                Fill = FillingMode.Disabled
            };

            IDataset<decimal> dataset2 = new LineDataset<decimal>()
            {
                Label = "Extruder",
                BackgroundColor = ColorUtil.FromDrawingColor(ChartColors.Red),
                BorderColor = ColorUtil.FromDrawingColor(ChartColors.Red),
                Fill = FillingMode.Disabled
            };

            //((List<string>)ChartConfig.Data.Labels).AddRange(Months.Take(InitalCount));
            ChartConfig.Data.Datasets.Add(dataset1);
            ChartConfig.Data.Datasets.Add(dataset2);
        }

        public override void OnPageHide()
        {
            _logger.LogInformation("HIDE");
            _temperatureManager.OnTemperatureChanged -= OnTemperatureChangedExecute;
        }
    }
}
