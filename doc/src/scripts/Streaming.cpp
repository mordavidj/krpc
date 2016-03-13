#include <krpc.hpp>
#include <krpc/services/krpc.hpp>
#include <krpc/services/space_center.hpp>
#include <iostream>

using namespace krpc::services;

int main() {
  krpc::Client conn = krpc::connect();
  KRPC krpc(&conn);
  SpaceCenter sc(&conn);
  SpaceCenter::Vessel vessel = sc.active_vessel();
  SpaceCenter::ReferenceFrame refframe = vessel.orbit().body().reference_frame();
  while (true) {
    std::tuple<double,double,double> pos = vessel.position(refframe);
    std::cout << std::get<0>(pos) << ","
              << std::get<1>(pos) << ","
              << std::get<2>(pos) << std::endl;
  }
}
