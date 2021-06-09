package reportward.nts.reportward.common;

import android.Manifest;
import android.content.Context;
import android.content.pm.PackageManager;
import android.location.Address;
import android.location.Geocoder;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Build;
import android.os.Bundle;
import android.widget.Toast;

import java.util.List;
import java.util.Locale;

import reportward.nts.reportward.model.LocationModel;

import static android.content.Context.LOCATION_SERVICE;

/**
 * Created by NTS-VANVV on 21/03/2019.
 */

public class LocationGpsListener implements LocationListener {
    public boolean isGPSEnabled = false;
    private Context mContext;
    private GPSTracker gpsTracker;

    private Location location;

    private final long MIN_DISTANCE_CHANGE_FOR_UPDATES = 10;
    private final long MIN_TIME_BW_UPDATES = 1000 * 60 * 1;

    protected LocationManager locationManager;

    public LocationGpsListener(Context context) {
        mContext = context;
        locationManager = (LocationManager) context.getSystemService(LOCATION_SERVICE);

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M && context.checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && context.checkSelfPermission(Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            Toast.makeText(context, "Chưa cho ứng dụng sử dụng vị trí trên thiết bị.", Toast.LENGTH_SHORT).show();
            isGPSEnabled = false;
            return;
        }

        gpsTracker = new GPSTracker(context);
        isGPSEnabled = true;
    }

//    public void setCurrentLocation(Context context) {
//        if (UtilityMethods.isGPSEnabled(context)) {
//            if (Build.VERSION.SDK_INT >= 23 &&
//                    ContextCompat.checkSelfPermission(context, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED &&
//                    ContextCompat.checkSelfPermission(context, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
//
//                requestPermissions(LOCATION_PERMS, LOCATION_REQUEST);
//                // return;
//            } else {
//                getCurrentAddress(context);
//            }
//
//        } else {
//            Toast.makeText(context, "Chưa cho ứng dụng sử dụng vị trí trên thiết bị.", Toast.LENGTH_SHORT).show();
//        }
//    }

    public LocationModel getCurrentAddress() {
        // Get the location manager
        LocationManager locationManager = (LocationManager) mContext.getSystemService(LOCATION_SERVICE);
        LocationModel locationModel = null;
        if (locationManager != null) {

//            try {
//
//                if (Build.VERSION.SDK_INT >= 23 && mContext.checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && mContext.checkSelfPermission(Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
//                    Toast.makeText(mContext, "Chưa cho ứng dụng sử dụng vị trí trên thiết bị.", Toast.LENGTH_SHORT).show();
//                    return null;
//                }
//                locationManager.requestLocationUpdates(
//                        LocationManager.NETWORK_PROVIDER,
//                        MIN_TIME_BW_UPDATES,
//                        MIN_DISTANCE_CHANGE_FOR_UPDATES, this);
//            } catch (Exception ex) {
//                Log.i("msg", "fail to request location update, ignore", ex);
//            }
//
//            if (locationManager != null) {
//                List<String> providers = locationManager.getProviders(true);
//                for (String provider : providers) {
//                    Location l = locationManager.getLastKnownLocation(provider);
//                    if (l == null) {
//                        continue;
//                    }
//                    if (location == null || l.getAccuracy() < location.getAccuracy()) {
//                        // Found best last known location: %s", l);
//                        location = l;
//                    }
//                }
//            }
            location = gpsTracker.getLocation();

            Geocoder gcd = new Geocoder(mContext,
                    Locale.getDefault());
            List<Address> addresses;
            try {
                Toast.makeText(mContext, "Vị trí: " + gpsTracker.getLatitude() + " / " + gpsTracker.getLongitude(), Toast.LENGTH_SHORT).show();
                addresses = gcd.getFromLocation(gpsTracker.getLatitude(),
                        gpsTracker.getLongitude(), 1);
                if (addresses.size() > 0) {
                    locationModel = new LocationModel();
                    locationModel.FullAddress = addresses.get(0).getAddressLine(0);
                    locationModel.ProvinceName = addresses.get(0).getAdminArea();
                    locationModel.DistrictName = addresses.get(0).getSubAdminArea();
                    locationModel.WardName = addresses.get(0).getLocality();
                    String subThoroughfare = addresses.get(0).getSubThoroughfare();
                    locationModel.Address = (Utils.isEmpty(subThoroughfare) ? "" : (subThoroughfare + " - ")) + addresses.get(0).getThoroughfare();
                } else {
                    Toast.makeText(mContext, "Lỗi không lấy được vị trí hiện tại của bạn.", Toast.LENGTH_SHORT).show();
                }

            } catch (Exception e) {
                e.printStackTrace();
                Toast.makeText(mContext, "Lỗi không lấy được vị trí người dùng.", Toast.LENGTH_SHORT).show();
            }
        }
        return locationModel;
    }

    public void stopLocationUpdates() {
        if (locationManager != null) {
            locationManager.removeUpdates(LocationGpsListener.this);
        }
    }

    public double getLatitude() {
        return location.getLatitude();
    }

    public double getLongitude() {
        return location.getLongitude();
    }

    @Override
    public void onLocationChanged(Location location) {
        this.location = location;
    }

    @Override
    public void onProviderDisabled(String provider) {
        // NO-OP
    }

    @Override
    public void onProviderEnabled(String provider) {
        // NO-OP
    }

    @Override
    public void onStatusChanged(String provider, int status, Bundle extras) {
        // NO-OP
    }
}
