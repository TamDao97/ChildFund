package reportward.nts.reportward.fragment;


import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.util.List;

import me.leolin.shortcutbadger.ShortcutBadger;
import reportward.nts.reportward.MainActivity;
import reportward.nts.reportward.R;
import reportward.nts.reportward.adapter.AdapterNotification;
import reportward.nts.reportward.common.Constants;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.InfoNotification;
import reportward.nts.reportward.model.LoginProfileModel;

import static android.content.Context.MODE_PRIVATE;

public class NotificationFragment extends Fragment {
    private View view;
    private View parent_view;
    private AdapterNotification adapterNotification;
    private List<InfoNotification> notifyModelList;
    private SharedPreferences prefsNotify;
    private RecyclerView recyclerView;
    private LoginProfileModel loginProfileModel;
    private SharedPreferences prefsLogin;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_notification, container, false);

        parent_view = getActivity().findViewById(android.R.id.content);

        //Get thông tin login
        prefsLogin = getActivity().getSharedPreferences(Constants.Key_Info_Login, getActivity().MODE_PRIVATE);
        String loginProfile = prefsLogin.getString(Constants.LoginProfile, null);
        if (loginProfile != null) {
            loginProfileModel = new Gson().fromJson(loginProfile, new TypeToken<LoginProfileModel>() {
            }.getType());
        }

        prefsNotify = getActivity().getSharedPreferences(Constants.Key_Notify + loginProfileModel.tenDangNhap, getActivity().MODE_PRIVATE);
        String current = prefsNotify.getString(Constants.ListNotify, "");
        notifyModelList = new Gson().fromJson(current, new TypeToken<List<InfoNotification>>() {
        }.getType());

        recyclerView = view.findViewById(R.id.list_Notification);

        adapterNotification = new AdapterNotification(getActivity(), notifyModelList);
        recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
        recyclerView.setAdapter(adapterNotification);

        TextView txtNoNotify = view.findViewById(R.id.txtNoNotify);
        if (notifyModelList == null || notifyModelList.size() == 0) {
            txtNoNotify.setVisibility(View.VISIBLE);
        } else {
            txtNoNotify.setVisibility(View.GONE);
        }

        adapterNotification.SetOnItemClickListener(new AdapterNotification.OnItemClickListener() {
            @Override
            public void onButtonSeenClick(View view, int position, InfoNotification obj) {
                FagmentDetailCatuvan fragment = new FagmentDetailCatuvan();
                fragment.soHoSo = obj.maSoCaTuVan;
                setViewStatus(obj.maSoCaTuVan, position);
                ((MainActivity) getActivity()).setTotalNotify();
                Utils.ChangeFragment(getActivity(), fragment, null);
            }

            @Override
            public void onButtonDeleteClick(View view, int position, InfoNotification obj) {

            }
        });

        return view;
    }

    /**
     * Sét trạng thái xem
     *
     * @param maSoCaTuVan
     */
    public void setViewStatus(String maSoCaTuVan, int position) {
        if (notifyModelList != null) {
            int index = 0;
            for (InfoNotification notify : notifyModelList) {
                if (notify.maSoCaTuVan.equals(maSoCaTuVan) && position == index) {
                    notify.viewStatus = true;
                    break;
                }
                index++;
            }
            prefsNotify.edit().putString(Constants.ListNotify, new Gson().toJson(notifyModelList)).commit();

            int numOfNotify = prefsNotify.getInt("NumOfNotify", 0);
            if (numOfNotify > 0)
                numOfNotify = numOfNotify - 1;

            // After that, you need to save the value again for another badge count
            SharedPreferences.Editor editor = prefsNotify.edit();
            editor.putInt(Constants.NumOfNotify, numOfNotify);
            editor.commit();
            ShortcutBadger.applyCount(parent_view.getContext(), numOfNotify);
            MainActivity.setTotalNotify();
        }
    }
}
