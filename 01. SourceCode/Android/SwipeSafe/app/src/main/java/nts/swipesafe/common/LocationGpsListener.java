package nts.swipesafe.common;

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
import android.support.v4.content.ContextCompat;
import android.util.Log;
import android.widget.Toast;

import java.util.List;
import java.util.Locale;

import nts.swipesafe.model.LocationModel;

import static android.content.Context.LOCATION_SERVICE;

/**
 * Created by NTS-VANVV on 21/03/2019.
 */

public class LocationGpsListener implements LocationListener {
    public boolean isPermissionGPS = false;
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

        if (Build.VERSION.SDK_INT >= 23 && context.checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && context.checkSelfPermission(Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            Toast.makeText(context, "Chưa cho ứng dụng sử dụng vị trí trên thiết bị.", Toast.LENGTH_SHORT).show();
            isPermissionGPS = false;
            return;
        }

        try {
            isGPSEnabled = locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER);
        } catch (Exception ex) {
        }

        gpsTracker = new GPSTracker(context);
        isPermissionGPS = true;
    }


    public LocationModel getCurrentAddress() {
        // Get the location manager
        LocationManager locationManager = (LocationManager) mContext.getSystemService(LOCATION_SERVICE);
        LocationModel locationModel = null;
        if (locationManager != null) {
            location = gpsTracker.getLocation();

            Geocoder gcd = new Geocoder(mContext,
                    Locale.getDefault());
            List<Address> addresses;
            try {
                addresses = gcd.getFromLocation(gpsTracker.getLatitude(),
                        gpsTracker.getLongitude(), 1);
                if (addresses.size() > 0) {
                    locationModel = new LocationModel();
                    locationModel.FullAddress = addresses.get(0).getAddressLine(0);
                    if (!Utils.isEmpty(locationModel.FullAddress)) {
                        try {
                            String[] address = locationModel.FullAddress.split(",");
                            locationModel.ProvinceName = address[address.length - 2].trim();
                            locationModel.DistrictName = address[address.length - 3].trim();
                            locationModel.WardName = address[address.length - 4].trim();
                            locationModel.Address = address[address.length - 5].trim();
                        } catch (Exception ex) {
                        }
                    }
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
