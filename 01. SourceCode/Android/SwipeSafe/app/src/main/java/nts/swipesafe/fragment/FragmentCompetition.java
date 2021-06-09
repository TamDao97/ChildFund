package nts.swipesafe.fragment;


import android.app.Dialog;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.os.Handler;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

import nts.swipesafe.R;

public class FragmentCompetition extends Fragment {
    private View view;
    private WebView webViewCompetition;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_competition, container, false);

        initComponent();
        confirmSecurity();
        // Inflate the layout for this fragment
        return view;
    }

    private void initComponent() {
        webViewCompetition = view.findViewById(R.id.webViewCompetition);
        webViewCompetition.setWebViewClient(new WebViewClient() {
            @Override
            public boolean shouldOverrideUrlLoading(WebView view, String url) {
                view.loadUrl(url);
                return false;
            }
        });
        webViewCompetition.getSettings().setJavaScriptEnabled(true);
        webViewCompetition.loadUrl("https://forms.office.com/Pages/ResponsePage.aspx?id=PBS9SA8yBUeKHDkp1wRFSKxA-zjxQ3lHvmvHZfEE2DxUOE1GWjk3QlhKSjNUTVQ2WlU0N1ExSkM2US4u");
    }

    public void confirmSecurity() {
        final Dialog dialog = new Dialog(getContext());
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        dialog.getWindow().getAttributes().windowAnimations = R.style.DialogTheme;
        dialog.setContentView(R.layout.popup_info_security);
        dialog.show();

        final Handler handler = new Handler();
        handler.postDelayed(new Runnable() {
            @Override
            public void run() {
                dialog.hide();
            }
        }, 15000);
    }
}
