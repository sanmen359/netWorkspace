using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Redis
{
    class RedisRepository
    {
        public void test() {

            RedisClient client = new RedisClient("172.21.0.192", 6379);
             
            //client.Add<Student>("StringEntity", stud);
            //Student Get_stud = client.Get<Student>("StringEntity");
        }
    }

}
