using System;

public static class CollisionManager
{
    public static Vector GetMaxDistance(GameObject mainBody, Vector velocity, out bool hasCollided)
    {
        Vector MaxDistance = velocity;
        hasCollided = false;

        if (mainBody.collider is BoxCollider)
        {
            foreach (GameObject collidingObject in mainBody.world.GetCollidingObjects())
            {
                if (collidingObject != mainBody)    //Не этот же объект
                {
                    //(дальность * 2)  >  (расстояние между центрами - радиус1 - радиус2)
                    if (2 * Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) > Math.Sqrt(Math.Pow(collidingObject.transform.position.X - mainBody.transform.position.X, 2) + Math.Pow(collidingObject.transform.position.Y - mainBody.transform.position.Y, 2)) - mainBody.collider.Radius - collidingObject.collider.Radius)   //ДОБАВИТЬ только релевантные коллайдеры
                    {
                        if (collidingObject.collider is BoxCollider)
                        {
                            if (CollideBoxes(mainBody, collidingObject, MaxDistance, out MaxDistance))
                                hasCollided = true;
                        }
                    }
                }
            }
        }

        return MaxDistance;
    }

    public static bool CollideBoxes(GameObject mainObject, GameObject obstacleObject, Vector velocity, out Vector maxVelocity)
    {
        BoxCollider mainCollider = mainObject.collider as BoxCollider;
        Vector MainCenter = mainObject.transform.position;
        BoxCollider obstacleCollider = obstacleObject.collider as BoxCollider;
        Vector ObstacleCenter = obstacleObject.transform.position;

        Vector[] Points = new Vector[mainCollider.Points.Length];
        Vector[] ObstaclePoints = new Vector[obstacleCollider.Points.Length];
        for (int i = 0; i < 4; i++)
        {
            Points[i] = MainCenter + mainCollider.Points[i];
            ObstaclePoints[i] = ObstacleCenter + obstacleCollider.Points[i];
        }

        bool hasCrossing = false;
        float moveDistance, maxMoveDistance;
        moveDistance = (float)Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2));
        maxMoveDistance = moveDistance;
        maxVelocity = new Vector(0, 0);

        for (int i = 0; i < Points.Length; i++) //все точки объекта
        {
            //if (mainCollider.Edges[i].Normal.X * velocity.X + mainCollider.Edges[i].Normal.Y * velocity.Y > 0)  //которые лежат на грани с нормалью ~ по направлению движения
            {
                foreach (Edge edge in obstacleCollider.Edges)   //все грани препятствия
                {
                    if (-(edge.Normal.X * velocity.X) - (edge.Normal.Y * velocity.Y) > 0) //ДОБАВИТЬ которые противоположны граням выше
                    {
                        if (FindCrossing(Points[i], new Vector(Points[i].X + velocity.X, Points[i].Y + velocity.Y), edge.Start + ObstacleCenter, edge.End + ObstacleCenter, out double crossingX, out double crossingY))
                        {
                            float length = (float)Math.Sqrt(Math.Pow(crossingX - Points[i].X, 2) + Math.Pow(crossingY - Points[i].Y, 2));

                            if (length < maxMoveDistance)
                                maxMoveDistance = length;

                            hasCrossing = true;
                        }
                    }
                }
            }
        }

        //Vector lastChecked = Points[Points.Length - 1];

        //foreach(Edge edge in mainCollider.Edges)
        //{
        //    if (edge.Normal.X * velocity.X + edge.Normal.Y * velocity.Y > 0)
        //    {
        //        foreach (Edge obstacleEdge in obstacleCollider.Edges)
        //        {
        //            if (-(obstacleEdge.Normal.X * velocity.X) - (obstacleEdge.Normal.Y * velocity.Y) > 0)
        //            {
        //                //if (lastChecked.X != edge.Start.X && lastChecked.Y != edge.Start.Y)
        //                //{
        //                //    if (FindCrossing(edge.Start, new Vector(edge.Start.X + velocity.X, edge.Start.Y + velocity.Y), obstacleEdge.Start + ObstacleCenter, obstacleEdge.End + ObstacleCenter, out double crossX, out double crossY))
        //                //    {
        //                //        float length = (float)Math.Sqrt(Math.Pow(crossX - edge.Start.X, 2) + Math.Pow(crossY - edge.Start.Y, 2));

        //                //        if (length < maxMoveDistance)
        //                //            maxMoveDistance = length;

        //                //        hasCrossing = true;
        //                //    }
        //                //}

        //                //if (FindCrossing(edge.End, new Vector(edge.End.X + velocity.X, edge.End.Y + velocity.Y), obstacleEdge.Start + ObstacleCenter, obstacleEdge.End + ObstacleCenter, out double crossingX, out double crossingY))
        //                //{
        //                //    float length = (float)Math.Sqrt(Math.Pow(crossingX - edge.Start.X, 2) + Math.Pow(crossingY - edge.Start.Y, 2));

        //                //    if (length < maxMoveDistance)
        //                //        maxMoveDistance = length;

        //                //    hasCrossing = true;
        //                //}

        //                //lastChecked = edge.End;

        //                if (FindCrossing(edge.Start, new Vector(edge.Start.X + velocity.X, edge.Start.Y + velocity.Y), obstacleEdge.Start + ObstacleCenter, obstacleEdge.End + ObstacleCenter, out double crossingX, out double crossingY))
        //                {
        //                    float length = (float)Math.Sqrt(Math.Pow(crossingX - edge.Start.X, 2) + Math.Pow(crossingY - edge.Start.Y, 2));

        //                    if (length < maxMoveDistance)
        //                        maxMoveDistance = length;

        //                    hasCrossing = true;
        //                }

        //                if (FindCrossing(edge.End, new Vector(edge.End.X + velocity.X, edge.End.Y + velocity.Y), obstacleEdge.Start + ObstacleCenter, obstacleEdge.End + ObstacleCenter, out double crossX, out double crossY))
        //                {
        //                    float length = (float)Math.Sqrt(Math.Pow(crossX - edge.End.X, 2) + Math.Pow(crossY - edge.End.Y, 2));

        //                    if (length < maxMoveDistance)
        //                        maxMoveDistance = length;

        //                    hasCrossing = true;
        //                }
        //            }
        //        }
        //    }
        //}

        for (int i = 0; i < ObstaclePoints.Length; i++) //все точки препятствия
        {
            //if (-(mainCollider.Edges[i].Normal.X * velocity.X) - (mainCollider.Edges[i].Normal.Y * velocity.Y) > 0) //ДОБАВИТЬ которые лежат на подходящей грани
            {
                foreach (Edge edge in mainCollider.Edges)   //все грани объекта
                {
                    if (edge.Normal.X * velocity.X + edge.Normal.Y * velocity.Y > 0) //ДОБАВИТЬ которые противоположны граням выше
                    {
                        if (FindCrossing(ObstaclePoints[i], new Vector(ObstaclePoints[i].X - velocity.X, ObstaclePoints[i].Y - velocity.Y), edge.Start + MainCenter, edge.End + MainCenter, out double crossingX, out double crossingY))
                        {
                            float length = (float) Math.Sqrt(Math.Pow(crossingX - ObstaclePoints[i].X, 2) + Math.Pow(crossingY - ObstaclePoints[i].Y, 2));

                            if (length < maxMoveDistance)
                                maxMoveDistance = length;

                            hasCrossing = true;
                        }
                    }
                }
            }
        }

        if (maxMoveDistance == 0)
        {
            maxVelocity = Vector.zero;
            return true;
        }
        else
        {
            maxVelocity.X = (velocity.X / (moveDistance / maxMoveDistance));
            maxVelocity.Y = (velocity.Y / (moveDistance / maxMoveDistance));
        }

        if (hasCrossing)    //чтобы встать рядом, а не на точку пересечения
        {
            float minimal = 1f / 100f;

            if (maxVelocity.X > 0)
            {
                if (maxVelocity.X > minimal)
                    maxVelocity.X -= minimal;
                else
                    maxVelocity.X = 0;
            }
            else if (maxVelocity.X < 0)
            {
                if (Math.Abs(maxVelocity.X) > minimal)
                    maxVelocity.X += minimal;
                else
                    maxVelocity.X = 0;
            }

            if (maxVelocity.Y > 0)
            {
                if (maxVelocity.Y > minimal)
                    maxVelocity.Y -= minimal;
                else
                    maxVelocity.Y = 0;
            }
            else if (maxVelocity.Y < 0)
            {
                if (Math.Abs(maxVelocity.Y) > minimal)
                    maxVelocity.Y -= minimal;
                else
                    maxVelocity.Y = 0;
            }
        }

        return hasCrossing;
    }

    static bool FindCrossing(Vector pointStart, Vector pointEnd, Vector obs1, Vector obs2, out double crossX, out double crossY)
    {
        double mult1;
        double mult2;

        mult1 = (pointEnd.X - pointStart.X) * (obs1.Y - pointStart.Y) - (pointEnd.Y - pointStart.Y) * (obs1.X - pointStart.X);
        mult2 = (pointEnd.X - pointStart.X) * (obs2.Y - pointStart.Y) - (pointEnd.Y - pointStart.Y) * (obs2.X - pointStart.X);

        if (mult1 == 0)
        {
            if (pointStart.X == obs1.X)
            {
                if (Math.Abs(obs1.Y - pointStart.Y) > 0)
                {
                    crossX = pointEnd.X;
                    crossY = pointEnd.Y;
                    return false;
                }
                else
                {
                    crossX = pointEnd.X;
                    crossY = pointEnd.Y;
                    return true;
                }
            }
            else if (pointStart.Y == obs1.Y)
            {
                if (Math.Abs(obs1.X - pointStart.X) > 0)
                {
                    crossX = pointEnd.X;
                    crossY = pointEnd.Y;
                    return false;
                }
                else
                {
                    crossX = pointEnd.X;
                    crossY = pointEnd.Y;
                    return true;
                }
            }
        }
        else if (mult2 == 0)
        {
            if (pointStart.X == obs2.X)
            {
                if (Math.Abs(obs2.Y - pointStart.Y) > 0)
                {
                    crossX = pointEnd.X;
                    crossY = pointEnd.Y;
                }
                else
                {
                    crossX = pointEnd.X;
                    crossY = pointEnd.Y;
                    return true;
                }
            }
            else if (pointStart.Y == obs2.Y)
            {
                if (Math.Abs(obs2.X - pointStart.X) > 0)
                {
                    crossX = pointEnd.X;
                    crossY = pointEnd.Y;
                }
                else
                {
                    crossX = pointEnd.X;
                    crossY = pointEnd.Y;
                    return true;
                }
            }
        }

        if (mult1 * mult2 < 0)
        {
            Vector temp = pointStart;
            pointStart = obs1;
            obs1 = pointEnd;
            pointEnd = obs2;
            obs2 = temp;

            mult1 = (pointEnd.X - pointStart.X) * (obs1.Y - pointStart.Y) - (pointEnd.Y - pointStart.Y) * (obs1.X - pointStart.X);
            mult2 = (pointEnd.X - pointStart.X) * (obs2.Y - pointStart.Y) - (pointEnd.Y - pointStart.Y) * (obs2.X - pointStart.X);


            if (mult1 == 0 || mult2 == 0)
            {
                crossX = obs1.X;
                crossY = obs1.Y;
                return true;
            }

            if (mult1 * mult2 < 0)
            {
                crossX = obs1.X + (obs2.X - obs1.X) * Math.Abs(mult1) / Math.Abs(mult2 - mult1);
                crossY = obs1.Y + (obs2.Y - obs1.Y) * Math.Abs(mult1) / Math.Abs(mult2 - mult1);
                return true;
            }
        }

        crossX = 0;
        crossY = 0;
        return false;
    }
}
