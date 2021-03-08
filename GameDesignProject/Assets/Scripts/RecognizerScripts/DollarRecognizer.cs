using System;
using System.Collections.Generic;


namespace Recognizer
{
    public class DollarRecognizer
    {
        //
        // one built-in unistroke per gesture type
        //
        private List<Unistroke> Unistrokes;
        private float AngleRange = MathUtility.Deg2Rad((float) 45.0);
        private float AnglePrecision = MathUtility.Deg2Rad((float) 2.0);
        private float HalfDiagonal = (float) (0.5 * Math.Sqrt(250.0 * 250.0 + 250.0 * 250.0));

        public DollarRecognizer()
        {
            this.Unistrokes = new List<Unistroke>()
            {
                new Unistroke("triangle",
                    new List<Point>()
                    {
                        new Point(137, 139), new Point(135, 141), new Point(133, 144), new Point(132, 146),
                        new Point(130, 149), new Point(128, 151), new Point(126, 155), new Point(123, 160),
                        new Point(120, 166), new Point(116, 171), new Point(112, 177), new Point(107, 183),
                        new Point(102, 188), new Point(100, 191), new Point(95, 195), new Point(90, 199),
                        new Point(86, 203), new Point(82, 206), new Point(80, 209), new Point(75, 213),
                        new Point(73, 213),
                        new Point(70, 216), new Point(67, 219), new Point(64, 221), new Point(61, 223),
                        new Point(60, 225),
                        new Point(62, 226), new Point(65, 225), new Point(67, 226), new Point(74, 226),
                        new Point(77, 227),
                        new Point(85, 229), new Point(91, 230), new Point(99, 231), new Point(108, 232),
                        new Point(116, 233), new Point(125, 233), new Point(134, 234), new Point(145, 233),
                        new Point(153, 232), new Point(160, 233), new Point(170, 234), new Point(177, 235),
                        new Point(179, 236), new Point(186, 237), new Point(193, 238), new Point(198, 239),
                        new Point(200, 237), new Point(202, 239), new Point(204, 238), new Point(206, 234),
                        new Point(205, 230), new Point(202, 222), new Point(197, 216), new Point(192, 207),
                        new Point(186, 198), new Point(179, 189), new Point(174, 183), new Point(170, 178),
                        new Point(164, 171), new Point(161, 168), new Point(154, 160), new Point(148, 155),
                        new Point(143, 150), new Point(138, 148), new Point(136, 148)
                    }),
                new Unistroke("x",
                    new List<Point>()
                    {
                        new Point(87, 142), new Point(89, 145), new Point(91, 148), new Point(93, 151),
                        new Point(96, 155),
                        new Point(98, 157), new Point(100, 160), new Point(102, 162), new Point(106, 167),
                        new Point(108, 169), new Point(110, 171), new Point(115, 177), new Point(119, 183),
                        new Point(123, 189), new Point(127, 193), new Point(129, 196), new Point(133, 200),
                        new Point(137, 206), new Point(140, 209), new Point(143, 212), new Point(146, 215),
                        new Point(151, 220), new Point(153, 222), new Point(155, 223), new Point(157, 225),
                        new Point(158, 223), new Point(157, 218), new Point(155, 211), new Point(154, 208),
                        new Point(152, 200), new Point(150, 189), new Point(148, 179), new Point(147, 170),
                        new Point(147, 158), new Point(147, 148), new Point(147, 141), new Point(147, 136),
                        new Point(144, 135), new Point(142, 137), new Point(140, 139), new Point(135, 145),
                        new Point(131, 152), new Point(124, 163), new Point(116, 177), new Point(108, 191),
                        new Point(100, 206), new Point(94, 217), new Point(91, 222), new Point(89, 225),
                        new Point(87, 226),
                        new Point(87, 224)
                    }),
                new Unistroke("rectangle",
                    new List<Point>()
                    {
                        new Point(78, 149), new Point(78, 153), new Point(78, 157), new Point(78, 160),
                        new Point(79, 162), new Point(79, 164), new Point(79, 167), new Point(79, 169),
                        new Point(79, 173),
                        new Point(79, 178), new Point(79, 183), new Point(80, 189), new Point(80, 193),
                        new Point(80, 198),
                        new Point(80, 202), new Point(81, 208), new Point(81, 210), new Point(81, 216),
                        new Point(82, 222),
                        new Point(82, 224), new Point(82, 227), new Point(83, 229), new Point(83, 231),
                        new Point(85, 230),
                        new Point(88, 232), new Point(90, 233), new Point(92, 232), new Point(94, 233),
                        new Point(99, 232),
                        new Point(102, 233), new Point(106, 233), new Point(109, 234), new Point(117, 235),
                        new Point(123, 236), new Point(126, 236), new Point(135, 237), new Point(142, 238),
                        new Point(145, 238), new Point(152, 238), new Point(154, 239), new Point(165, 238),
                        new Point(174, 237), new Point(179, 236), new Point(186, 235), new Point(191, 235),
                        new Point(195, 233), new Point(197, 233), new Point(200, 233), new Point(201, 235),
                        new Point(201, 233), new Point(199, 231), new Point(198, 226), new Point(198, 220),
                        new Point(196, 207), new Point(195, 195), new Point(195, 181), new Point(195, 173),
                        new Point(195, 163), new Point(194, 155), new Point(192, 145), new Point(192, 143),
                        new Point(192, 138), new Point(191, 135), new Point(191, 133), new Point(191, 130),
                        new Point(190, 128), new Point(188, 129), new Point(186, 129), new Point(181, 132),
                        new Point(173, 131), new Point(162, 131), new Point(151, 132), new Point(149, 132),
                        new Point(138, 132), new Point(136, 132), new Point(122, 131), new Point(120, 131),
                        new Point(109, 130), new Point(107, 130), new Point(90, 132), new Point(81, 133),
                        new Point(76, 133)
                    }),
                new Unistroke("circle",
                    new List<Point>()
                    {
                        new Point(127, 141), new Point(124, 140), new Point(120, 139), new Point(118, 139),
                        new Point(116, 139), new Point(111, 140), new Point(109, 141), new Point(104, 144),
                        new Point(100, 147), new Point(96, 152), new Point(93, 157), new Point(90, 163),
                        new Point(87, 169),
                        new Point(85, 175), new Point(83, 181), new Point(82, 190), new Point(82, 195),
                        new Point(83, 200),
                        new Point(84, 205), new Point(88, 213), new Point(91, 216), new Point(96, 219),
                        new Point(103, 222),
                        new Point(108, 224), new Point(111, 224), new Point(120, 224), new Point(133, 223),
                        new Point(142, 222), new Point(152, 218), new Point(160, 214), new Point(167, 210),
                        new Point(173, 204), new Point(178, 198), new Point(179, 196), new Point(182, 188),
                        new Point(182, 177), new Point(178, 167), new Point(170, 150), new Point(163, 138),
                        new Point(152, 130), new Point(143, 129), new Point(140, 131), new Point(129, 136),
                        new Point(126, 139)
                    }),
                new Unistroke("check",
                    new List<Point>()
                    {
                        new Point(91, 185), new Point(93, 185), new Point(95, 185), new Point(97, 185),
                        new Point(100, 188), new Point(102, 189), new Point(104, 190), new Point(106, 193),
                        new Point(108, 195), new Point(110, 198), new Point(112, 201), new Point(114, 204),
                        new Point(115, 207), new Point(117, 210), new Point(118, 212), new Point(120, 214),
                        new Point(121, 217), new Point(122, 219), new Point(123, 222), new Point(124, 224),
                        new Point(126, 226), new Point(127, 229), new Point(129, 231), new Point(130, 233),
                        new Point(129, 231), new Point(129, 228), new Point(129, 226), new Point(129, 224),
                        new Point(129, 221), new Point(129, 218), new Point(129, 212), new Point(129, 208),
                        new Point(130, 198), new Point(132, 189), new Point(134, 182), new Point(137, 173),
                        new Point(143, 164), new Point(147, 157), new Point(151, 151), new Point(155, 144),
                        new Point(161, 137), new Point(165, 131), new Point(171, 122), new Point(174, 118),
                        new Point(176, 114), new Point(177, 112), new Point(177, 114), new Point(175, 116),
                        new Point(173, 118)
                    }),
                new Unistroke("caret",
                    new List<Point>()
                    {
                        new Point(79, 245), new Point(79, 242), new Point(79, 239), new Point(80, 237),
                        new Point(80, 234), new Point(81, 232), new Point(82, 230), new Point(84, 224),
                        new Point(86, 220),
                        new Point(86, 218), new Point(87, 216), new Point(88, 213), new Point(90, 207),
                        new Point(91, 202),
                        new Point(92, 200), new Point(93, 194), new Point(94, 192), new Point(96, 189),
                        new Point(97, 186),
                        new Point(100, 179), new Point(102, 173), new Point(105, 165), new Point(107, 160),
                        new Point(109, 158), new Point(112, 151), new Point(115, 144), new Point(117, 139),
                        new Point(119, 136), new Point(119, 134), new Point(120, 132), new Point(121, 129),
                        new Point(122, 127), new Point(124, 125), new Point(126, 124), new Point(129, 125),
                        new Point(131, 127), new Point(132, 130), new Point(136, 139), new Point(141, 154),
                        new Point(145, 166), new Point(151, 182), new Point(156, 193), new Point(157, 196),
                        new Point(161, 209), new Point(162, 211), new Point(167, 223), new Point(169, 229),
                        new Point(170, 231), new Point(173, 237), new Point(176, 242), new Point(177, 244),
                        new Point(179, 250), new Point(181, 255), new Point(182, 257)
                    })
            };
        }

        //
        // The $1 Gesture Recognizer API begins here -- 3 methods: Recognize(), AddGesture(), and DeleteUserGestures()
        //
        public Result Recognize(List<Point> points)
        {
            var candidate = new Unistroke("", points);
            var u = -1;

            var b = Single.PositiveInfinity;
            for (var i = 0;
                i < this.Unistrokes.Count;
                i++) // for each unistroke template
            {
                var d = MathUtility.DistanceAtBestAngle(candidate.Points, this.Unistrokes[i], -AngleRange, +AngleRange,
                    AnglePrecision); // Golden Section Search (original $1)
                if (d < b)
                {
                    b = d; // best (least) distance
                    u = i; // unistroke index
                }
            }

            return (u == -1)
                ? new Result("No match.", (float) 0.0)
                : new Result(this.Unistrokes[u].Name, (float) (1.0 - b / HalfDiagonal));
        }

        /*this.AddGesture = function(name, points)
        {
            this.Unistrokes[this.Unistrokes.length] = new Unistroke(name, points); // append new unistroke

            var num = 0;
            for (var i = 0;
                i < this.Unistrokes.length;
                i++)
            {
                if (this.Unistrokes[i].Name == name)
                    num++;
            }

            return num;
        }
        this.DeleteUserGestures = function()
        {
            this.Unistrokes.length = NumUnistrokes; // clear any beyond the original set
            return NumUnistrokes;
        }*/
    }
}